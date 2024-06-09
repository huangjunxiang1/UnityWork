@echo off
path = %path%;%~dp0\..\Client\Tools\PBCodeGen\win-x64\;

set protoPath=%~dp0\main
set outputPath=%~dp0..\Client\Client\Assets\Code\Main\_Gen\PB
PBCodeGen.exe false

set protoPath=%~dp0\hot
set outputPath=%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\PB
PBCodeGen.exe false

pause