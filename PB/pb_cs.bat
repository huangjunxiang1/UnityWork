@echo off
path = %path%;.\..\Client\PBCodeGen\PBCodeGen\bin\Debug\net6.0\;
PBCodeGen.exe false %~dp0\main %~dp0..\Client\Client\Assets\Code\Main\_Gen\PB
PBCodeGen.exe false %~dp0\hot %~dp0..\Client\Client\Assets\Code\HotFix\Game\_Gen\PB
pause