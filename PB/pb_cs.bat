@echo off

REM 环境说明
REM dotnet tool install --global protobuf-net.Protogen --version 3.4.40
set PROTOC_EXE=protogen

REM ==========================================
REM 处理 main 目录
REM ==========================================
set protoPath=%~dp0\main
set outputPath=%~dp0..\Client\Client\Assets\Code\Main\_Gen\PB

if not exist "%outputPath%" mkdir "%outputPath%"
del /Q /F "%outputPath%\*.cs" >nul 2>&1

pushd "%protoPath%"
%PROTOC_EXE% --proto_path=. --csharp_out="%outputPath%" *.proto
if %errorlevel% neq 0 (
    echo [ERROR] Main project generation failed!
    popd
    pause
    exit /b 1
)
popd

REM ==========================================
REM 处理 hot 目录
REM ==========================================
set protoPath=%~dp0\hot
set outputPath=%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\PB

if not exist "%outputPath%" mkdir "%outputPath%"
del /Q /F "%outputPath%\*.cs" >nul 2>&1

pushd "%protoPath%"
%PROTOC_EXE% --proto_path=. --csharp_out="%outputPath%" *.proto
if %errorlevel% neq 0 (
    echo [ERROR] Hotfix project generation failed!
    popd
    pause
    exit /b 1
)
popd

REM ==========================================
REM 调用 PowerShell 脚本清洗代码
REM ==========================================
echo.
echo [INFO] 正在清洗代码，移除 global:: 前缀...

REM 绕过执行策略限制，安全调用同目录下的 clean_pb.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0clean_pb.ps1" -TargetDir "%~dp0..\Client\Client\Assets\Code\Main\_Gen\PB"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0clean_pb.ps1" -TargetDir "%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\PB"

echo [SUCCESS] All code generated and cleaned successfully!
pause