@echo off
path= %~dp0;%~dp0\..\..\..\Client\Tools\ExcelToDB\win-x64\;

set type=CombineLanguage
set assetsPath=%~dp0..\..\..\Client\Client\Assets\Res\Config\raw\Tabs\
set excelPath=%~dp0\
set ClearBytes=false
ExcelToDB.exe false

pause