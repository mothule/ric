@echo off
set HOME_DIR="RIC.CLI\bin\Release"
set RIC="RIC.exe"
set SP="sprint5"
cd %HOME_DIR%
%RIC% /get-sum-hours %SP%
%RIC% /get-remain-hours %SP%
pause