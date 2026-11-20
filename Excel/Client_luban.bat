@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion
cd /d "%~dp0"

set "Root=%~dp0"
set "LUBAN_DLL=%Root%..\Client\Tools\Luban\Luban.dll"
set "CODE_TARGET=cs-bin"
set "DATA_TARGET=bin"

:: ==========================================
:: 1. 生成 clientNative 配置与 Native 代码
:: ==========================================
set "NATIVE_TARGET=clientNative"
set "NATIVE_CODE_OUT=%Root%..\Client\Client\Assets\Code\Main\_Gen\Luban"
set "NATIVE_DATA_OUT=%Root%..\Client\Client\Assets\Res\Config\raw\Tabs\Native"

echo 正在生成 [%NATIVE_TARGET%] 配置（动态多语言模式，仅生成一份数据）...
dotnet "%LUBAN_DLL%" ^
    -t %NATIVE_TARGET% ^
    -c %CODE_TARGET% ^
    -d %DATA_TARGET% ^
    --conf "%Root%Client_luban.conf" ^
    --customTemplateDir "%Root%Client_Templates_Native" ^
    -x outputCodeDir="%NATIVE_CODE_OUT%" ^
    -x outputDataDir="%NATIVE_DATA_OUT%"

if !errorlevel! neq 0 (
    echo ❌ [%NATIVE_TARGET%] 生成失败，错误码：!errorlevel!
    pause
    exit /b !errorlevel!
)

echo ✅ [%NATIVE_TARGET%] 生成成功，开始执行后处理脚本...
powershell -NoProfile -ExecutionPolicy Bypass -File "%Root%Client_luban_toNative.ps1" -CodeDir "%NATIVE_CODE_OUT%"
if !errorlevel! neq 0 (
    echo ❌ lubanNative.ps1 执行失败，错误码：!errorlevel!
    pause
    exit /b !errorlevel!
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%Root%Client_luban_ConvertVector.ps1" -CodeDir "%NATIVE_CODE_OUT%"
if !errorlevel! neq 0 (
    echo ❌ [%NATIVE_TARGET%] lubanNative_ConverVector.ps1 执行失败，错误码：!errorlevel!
    pause
    exit /b !errorlevel!
)

echo.
:: ==========================================
:: 2. 生成 client 热更配置
:: ==========================================
set "CLIENT_TARGET=client"
set "CLIENT_CODE_OUT=%Root%..\Client\Client\Assets\Code\HotFix\_Gen\Luban"
set "CLIENT_DATA_OUT=%Root%..\Client\Client\Assets\Res\Config\raw\Tabs\%DATA_TARGET%"

echo 正在生成 [%CLIENT_TARGET%] 配置（动态多语言模式，仅生成一份数据）...
dotnet "%LUBAN_DLL%" ^
    -t %CLIENT_TARGET% ^
    -c %CODE_TARGET% ^
    -d %DATA_TARGET% ^
    --conf "%Root%Client_luban.conf" ^
    -x outputCodeDir="%CLIENT_CODE_OUT%" ^
    -x outputDataDir="%CLIENT_DATA_OUT%" ^
    -x l10n.provider=default ^
    -x l10n.textFile.path="%Root%l10n/text.xlsx" ^
    -x l10n.textFile.keyFieldName=key

if !errorlevel! neq 0 (
    echo ❌ [%CLIENT_TARGET%] 生成失败，错误码：!errorlevel!
    pause
    exit /b !errorlevel!
)

echo ✅ [%CLIENT_TARGET%] 生成成功！数据中 text 字段已保留为 Key。
powershell -NoProfile -ExecutionPolicy Bypass -File "%Root%Client_luban_ConvertVector.ps1" -CodeDir "%CLIENT_CODE_OUT%"
if !errorlevel! neq 0 (
    echo ❌ [%CLIENT_TARGET%] lubanNative_ConverVector.ps1 执行失败，错误码：!errorlevel!
    pause
    exit /b !errorlevel!
)

echo.
echo ✅ 全部流程执行完成！
pause