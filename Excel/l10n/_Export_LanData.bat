@echo off
path= %~dp0;%~dp0\..\..\Client\Tools\ExcelToDB\win-x64\;

set excelPath=%~dp0\
set type=ExcelToLanguage
set assetsPath=%~dp0\..\..\Client\Client\Assets\Res\Config\raw\Tabs\Lan\
ExcelToDB.exe false

pause