@echo off
set compress=true
set excelPath=%~dp0
set assetPath=%~dp0..\Client\Client\Assets\
path= %~dp0;%~dp0\..\Client\ExcelToDB\ExcelToDB\bin\Debug\net6.0\;
ExcelToDB.exe false
pause