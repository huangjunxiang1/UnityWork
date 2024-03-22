@echo off
set compress=true
set excelPath=%~dp0

set mainCodePath=%~dp0..\Client\Client\Assets\Code\Main\_Gen\
set hotCodePath=%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\
set assetsPath=%~dp0..\Client\Client\Assets\Res\Config\Tabs\
set assetsPath2=%~dp0..\Server\Tabs\

path= %~dp0;%~dp0\..\Client\ExcelToDB\ExcelToDB\bin\Debug\net6.0\;
ExcelToDB.exe false
pause