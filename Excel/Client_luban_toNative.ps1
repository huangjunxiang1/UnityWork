param(
    [Parameter(Mandatory = $true)]
    [string]$CodeDir
)

if (-not (Test-Path -Path $CodeDir)) {
    Write-Error "CodeDir not found: $CodeDir"
    exit 1
}

function Add-UnsafeUsings {
    param(
        [string]$Code,
        [string]$Eol
    )

    if ($Code -notmatch '\bUnsafe(List|HashMap|HashSet)<') {
        return $Code
    }

    $normalized = $Code -replace "`r`n", "`n" -replace "`r", "`n"
    $lines = [System.Collections.Generic.List[string]]($normalized -split "`n")

    $hasCollections = $false
    $hasUnsafe = $false
    $lastUsingIndex = -1

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $trim = $lines[$i].Trim()
        if ($trim -eq "using Unity.Collections;") { $hasCollections = $true }
        if ($trim -eq "using Unity.Collections.LowLevel.Unsafe;") { $hasUnsafe = $true }
        if ($trim -match '^using\s+[^;]+;\s*$') { $lastUsingIndex = $i }
    }

    $inserts = New-Object System.Collections.Generic.List[string]
    if (-not $hasCollections) { $inserts.Add("using Unity.Collections;") }
    if (-not $hasUnsafe) { $inserts.Add("using Unity.Collections.LowLevel.Unsafe;") }

    if ($inserts.Count -gt 0) {
        $pos = if ($lastUsingIndex -ge 0) { $lastUsingIndex + 1 } else { 0 }
        for ($i = 0; $i -lt $inserts.Count; $i++) {
            $lines.Insert($pos + $i, $inserts[$i])
        }
    }

    return (($lines -join "`n") -replace "`n", $Eol)
}

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$count = 0

Get-ChildItem -Path $CodeDir -Filter "*.cs" -Recurse -File | ForEach-Object {
    $raw = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
    if ([string]::IsNullOrEmpty($raw)) { return }

    $eol = if ($raw.Contains("`r`n")) { "`r`n" } else { "`n" }
    $text = $raw

    # 1. 容器与数组类型替换
    $text = [regex]::Replace(
        $text,
        "\b(?:System\.Collections\.Generic\.)?(?:Dictionary|IDictionary|IReadOnlyDictionary)<([^,\r\n<>]+?)\s*,\s*([^>\r\n]+?)>",
        "UnsafeHashMap<`$1, `$2>"
    )
    $text = [regex]::Replace(
        $text,
        "\b(?:System\.Collections\.Generic\.)?(?:List|IList|IReadOnlyList)<([^>\r\n]+?)>",
        "UnsafeList<`$1>"
    )
    $text = [regex]::Replace(
        $text,
        "\b(?:System\.Collections\.Generic\.)?(?:HashSet|ISet|IReadOnlySet)<([^>\r\n]+?)>",
        "UnsafeHashSet<`$1>"
    )
    $text = [regex]::Replace(
        $text,
        "\b([A-Za-z_][A-Za-z0-9_.]*)\[\]",
        "UnsafeList<`$1>"
    )

    # 2. 构造函数与数组分配补 Allocator.Persistent（自动剥离 /*...*/ 注释并保留容量参数）
    $text = [regex]::Replace(
        $text,
        "\bnew\s+UnsafeHashMap<([^,\r\n<>]+?)\s*,\s*([^>\r\n]+?)>\(([^()\r\n]*)\)",
        {
            param($m)
            $t1 = $m.Groups[1].Value.Trim()
            $t2 = $m.Groups[2].Value.Trim()
            $arg = [regex]::Replace($m.Groups[3].Value, '/\*.*?\*/', '').Trim()
            if ([string]::IsNullOrEmpty($arg)) {
                "new UnsafeHashMap<$t1, $t2>(16, Allocator.Persistent)"
            } elseif ($arg -match '\bAllocator\.') {
                "new UnsafeHashMap<$t1, $t2>($arg)"
            } else {
                "new UnsafeHashMap<$t1, $t2>($arg, Allocator.Persistent)"
            }
        }
    )

    $text = [regex]::Replace(
        $text,
        "\bnew\s+UnsafeList<([^>\r\n]+?)>\(([^()\r\n]*)\)",
        {
            param($m)
            $t1 = $m.Groups[1].Value.Trim()
            $arg = [regex]::Replace($m.Groups[2].Value, '/\*.*?\*/', '').Trim()
            if ([string]::IsNullOrEmpty($arg)) {
                "new UnsafeList<$t1>(16, Allocator.Persistent)"
            } elseif ($arg -match '\bAllocator\.') {
                "new UnsafeList<$t1>($arg)"
            } else {
                "new UnsafeList<$t1>($arg, Allocator.Persistent)"
            }
        }
    )

 $text = [regex]::Replace(
    $text,
    "\bnew\s+UnsafeHashSet<([^>\r\n]+?)>\(([^()\r\n]*)\)",
    {
        param($m)
        $t1 = $m.Groups[1].Value.Trim()
        # 只去掉 /* 和 */，保留里面的表达式（如 n0 * 3 / 2）
        $arg = [regex]::Replace($m.Groups[2].Value, '/\*|\*/', '').Trim()
        if ([string]::IsNullOrEmpty($arg)) {
            "new UnsafeHashSet<$t1>(16, Allocator.Persistent)"
        } elseif ($arg -match '\bAllocator\.') {
            "new UnsafeHashSet<$t1>($arg)"
        } else {
            "new UnsafeHashSet<$t1>($arg, Allocator.Persistent)"
        }
    }
)

    $text = [regex]::Replace(
        $text,
        "\bnew\s+([A-Za-z_][A-Za-z0-9_.]*)\[([^\[\]\r\n]+)\]",
        {
            param($m)
            $elemType = $m.Groups[1].Value.Trim()
            $sizeExpr = [regex]::Replace($m.Groups[2].Value, '/\*.*?\*/', '').Trim()
            if ($sizeExpr -match '\bAllocator\.') {
                "new UnsafeList<$elemType>($sizeExpr)"
            } else {
                "new UnsafeList<$elemType>($sizeExpr, Allocator.Persistent)"
            }
        }
    )

    # 3. 索引赋值改用 .Add(...)（不直接 set）
    $text = [regex]::Replace(
        $text,
        "\b([A-Za-z_][A-Za-z0-9_]*)\[([^\[\]\r\n]+)\]\s*=(?!=)\s*([^;\r\n]+);",
        "`$1.Add(`$3);"
    )

    # 4. 添加 using
    $text = Add-UnsafeUsings -Code $text -Eol $eol

    if ($text -ne $raw) {
        [System.IO.File]::WriteAllText($_.FullName, $text, $utf8NoBom)
        $count++
        Write-Host "Updated: $($_.FullName)"
    }
}

Write-Host "Done. Modified $count files."