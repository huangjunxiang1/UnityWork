@echo off
path= %~dp0;%~dp0\..\Client\Tools\ExcelToDB\win-x64\;

set compress=true
set type=0
set assetsPath=%~dp0..\Client\Client\Assets\Res\Config\Tabs\

set codePath=%~dp0..\Client\Client\Assets\Code\Main\_Gen\
set excelPath=%~dp0\main\
set TabName=TabM
set genMapping=true
set genEcs=true
set ClearBytes=true

ExcelToDB.exe false

set ClearBytes=false
set codePath=%~dp0..\Client\Client\Assets\Code\HotFix\_Gen\
set excelPath=%~dp0\hot\
set TabName=TabL
set genMapping=false
set genEcs=false

ExcelToDB.exe false

set excelPath=%~dp0\Language\
set type=1
ExcelToDB.exe false

pause