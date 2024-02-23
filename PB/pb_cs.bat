@echo off
path = %path%;%~dp0\..\Client\PBCodeGen\PBCodeGen\bin\Debug\net6.0\;

set protoPath=%~dp0\main
set outputPath=%~dp0..\Client\Client\Assets\Code\Main\_Gen\PB
PBCodeGen.exe false

set protoPath=%~dp0\hot
set outputPath=%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\PB
PBCodeGen.exe false

pause