param (
    [string]$TargetDir
)

if (-not (Test-Path $TargetDir)) {
    exit 0
}

$files = Get-ChildItem -Path $TargetDir -Filter *.cs
$protoMap = @{}

Write-Host "`n=== 1. Start Scanning Code ==="

# ==========================================
# 阶段一：预扫描，收集所有的 C2S 和 S2C 配对关系
# ==========================================
foreach ($file in $files) {
    if ($file.Name -eq "PBMessageMeta.cs") { continue }

    $content = [System.IO.File]::ReadAllText($file.FullName)
    
    $regex = '\[(?:global::ProtoBuf\.)?ProtoContract(?:\(Name\s*=\s*@"(.*?)"\))?\][\s\S]*?public\s+partial\s+class\s+(\w+)'
    $matches = [regex]::Matches($content, $regex)
    foreach ($m in $matches) {
        if ($m.Groups[1].Success) {
            $protoName = $m.Groups[1].Value.ToLower()
            $className = $m.Groups[2].Value
            $protoMap[$protoName] = $className
        }
    }
}

$injectList = @{}
foreach ($key in $protoMap.Keys) {
    if ($key -match '^c2s_') {
        $s2cKey = $key -replace '^c2s_', 's2c_'
        if ($protoMap.ContainsKey($s2cKey)) {
            $c2sClass = $protoMap[$key]
            $s2cClass = $protoMap[$s2cKey]
            $injectList[$c2sClass] = $s2cClass
        }
    }
}

# ==========================================
# 阶段二：正式清洗代码，替换基类，剔除冗余接口实现
# ==========================================
Write-Host "=== 2. Start Cleaning and Injecting ==="

foreach ($file in $files) {
    if ($file.Name -eq "PBMessageMeta.cs") { continue }

    $content = [System.IO.File]::ReadAllText($file.FullName)
    
    # 1. 清理 global:: 前缀及括号
    $content = $content -replace '\[global::ProtoBuf\.', '['
    $content = $content -replace 'global::ProtoBuf\.', ''
    $content = $content -replace 'global::System\.Collections\.Generic\.', ''
    $content = $content -replace 'global::System\.ComponentModel\.', ''
    $content = $content -replace 'global::System\.', ''
    $content = $content -replace 'global::', '' 
    $content = $content -replace 'ProtoContract\(\)', 'ProtoContract'
    $content = $content -replace 'ProtoMap\(\)', 'ProtoMap'
    
    # 2. 插入 using (如果你有基类所在的其他命名空间，记得在这里的字符串里加上)
    $usingStr = "using System;`r`nusing System.Collections.Generic;`r`nusing System.ComponentModel;`r`nusing ProtoBuf;`r`n"
    if ($content -match '(namespace\s+)') {
        $content = $content -replace '(namespace\s+)', "$usingStr`r`n`$1"
    } elseif ($content -match '(#region\s+Designer\s+generated\s+code)') {
        $content = $content -replace '(#region\s+Designer\s+generated\s+code)', "`$1`r`n`r`n$usingStr"
    }

    # ==========================================
    # 3. 【核心修改 1】：替换继承为 Message 基类
    # ==========================================
    $content = $content -replace '(public\s+partial\s+class\s+\w+)\s*:\s*IExtensible', '$1 : Message'

    # ==========================================
    # 4. 【核心修改 2】：剔除冗长的 IExtensible 实现（修复缩进）
    # ==========================================
    $content = $content -replace '(?s)\r?\n[ \t]*private\s+IExtension\s+__pbn__extensionData;\s*IExtension\s+IExtensible\.GetExtensionObject\(bool\s+createIfMissing\)\s*=>\s*Extensible\.GetExtensionObject\(ref\s+__pbn__extensionData,\s*createIfMissing\);(\r?\n)*', "`r`n"

    # ==========================================
    # 5. 【核心注入】消息标签
    # ==========================================
    foreach ($c2sClass in $injectList.Keys) {
        $s2cClass = $injectList[$c2sClass]
        $pattern = "(?m)^([ \t]*)(public\s+partial\s+class\s+$c2sClass\b)"
        $replacement = "`$1[Message(0, typeof($s2cClass))]`r`n`$1`$2"
        $oldContent = $content
        $content = $content -replace $pattern, $replacement
        if ($oldContent -ne $content) {
            Write-Host "  Injected: [Message] -> $c2sClass" -ForegroundColor Green
        }
    }

    # 6. 写回文件
    [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
}

Write-Host "`n[SUCCESS] Code cleaned, inherited Message base class!" -ForegroundColor Cyan