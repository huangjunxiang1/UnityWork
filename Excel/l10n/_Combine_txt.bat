@echo off
path= %~dp0;%~dp0\..\..\Client\Tools\ExcelToDB\win-x64\;

set type=CombineLanguage
set excelPath=%~dp0\
ExcelToDB.exe false

pause