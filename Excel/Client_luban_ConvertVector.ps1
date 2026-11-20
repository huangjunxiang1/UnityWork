param(
    [Parameter(Mandatory = $true)]
    [string]$CodeDir
)

if (-not (Test-Path -Path $CodeDir)) {
    Write-Error "CodeDir not found: $CodeDir"
    exit 1
}

# 1. Remove generated files for float2~4 / int2~4
$removeNames = @('float2', 'float3', 'float4', 'int2', 'int3', 'int4')
foreach ($name in $removeNames) {
    Get-ChildItem -Path $CodeDir -Recurse -File -Filter ('*' + $name + '*.cs') | ForEach-Object {
        Remove-Item -Path $_.FullName -Force
        Write-Host "Deleted: $($_.FullName)"
    }
}

function Expand-VectorDeserialize {
    param(
        [string]$TypeName,
        [string]$BufExpr
    )

    if ([string]::IsNullOrWhiteSpace($BufExpr)) {
        $BufExpr = '_buf'
    }

    switch ($TypeName) {
        'float2' { return "new float2($BufExpr.ReadFloat(), $BufExpr.ReadFloat())" }
        'float3' { return "new float3($BufExpr.ReadFloat(), $BufExpr.ReadFloat(), $BufExpr.ReadFloat())" }
        'float4' { return "new float4($BufExpr.ReadFloat(), $BufExpr.ReadFloat(), $BufExpr.ReadFloat(), $BufExpr.ReadFloat())" }
        'int2'   { return "new int2($BufExpr.ReadInt(), $BufExpr.ReadInt())" }
        'int3'   { return "new int3($BufExpr.ReadInt(), $BufExpr.ReadInt(), $BufExpr.ReadInt())" }
        'int4'   { return "new int4($BufExpr.ReadInt(), $BufExpr.ReadInt(), $BufExpr.ReadInt(), $BufExpr.ReadInt())" }
        default  { return $null }
    }
}

function Remove-LubanConvertWrapper {
    param([string]$Text)

    $pattern = 'LubanConvert\.Convert\('
    while ($true) {
        $m = [regex]::Match($Text, $pattern)
        if (-not $m.Success) { break }

        $startIdx = $m.Index
        $parenStart = $m.Index + $m.Length - 1
        $depth = 0
        $endIdx = -1

        for ($i = $parenStart; $i -lt $Text.Length; $i++) {
            $ch = $Text[$i]
            if ($ch -eq '(') {
                $depth++
            } elseif ($ch -eq ')') {
                $depth--
                if ($depth -eq 0) {
                    $endIdx = $i
                    break
                }
            }
        }

        if ($endIdx -lt 0) { break }

        $inner = $Text.Substring($parenStart + 1, $endIdx - $parenStart - 1)
        $Text = $Text.Substring(0, $startIdx) + $inner + $Text.Substring($endIdx + 1)
    }

    return $Text
}

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$modifiedCount = 0

Get-ChildItem -Path $CodeDir -Filter '*.cs' -Recurse -File | ForEach-Object {
    $raw = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
    if ([string]::IsNullOrEmpty($raw)) { return }

    $eol = if ($raw.Contains("`r`n")) { "`r`n" } else { "`n" }
    $updated = $raw

    $updated = Remove-LubanConvertWrapper -Text $updated

    $updated = [regex]::Replace(
        $updated,
        '(?:[A-Za-z_][A-Za-z0-9_.:]*\.)?Deserialize(float2|float3|float4|int2|int3|int4)\s*\(\s*([^()]*?)\s*\)',
        {
            param($m)
            $vecType = $m.Groups[1].Value
            $bufArg  = $m.Groups[2].Value
            $expanded = Expand-VectorDeserialize -TypeName $vecType -BufExpr $bufArg
            if ($expanded) { return $expanded } else { return $m.Value }
        }
    )

    $updated = [regex]::Replace(
        $updated,
        '[A-Za-z_][A-Za-z0-9_.:]*\.(new\s+(?:float2|float3|float4|int2|int3|int4)\s*\()',
        '$1'
    )

    if ($updated -match '\bnew\s+(float2|float3|float4|int2|int3|int4)\(') {
        if ($updated -notmatch '(?m)^using\s+Unity\.Mathematics\s*;') {
            $updated = [regex]::Replace(
                $updated,
                '(?m)^(using\s+Luban\s*;\s*\r?\n)',
                "`$1using Unity.Mathematics;$eol",
                1
            )
        }
    }

    if ($updated -ne $raw) {
        [System.IO.File]::WriteAllText($_.FullName, $updated, $utf8NoBom)
        $modifiedCount++
        Write-Host "Updated: $($_.FullName)"
    }
}

Write-Host "Done. Modified $modifiedCount .cs files."