@echo off
set allureResultsPath="C:\Users\vasudev.singh\source\repos\demoTest\demoTest\bin\Debug\net6.0\allure-results"

echo Clearing old Allure results at: %allureResultsPath%
if exist "%allureResultsPath%\*" (
    del /Q "%allureResultsPath%\*"
    echo Old Allure results cleared successfully.
) else (
    echo The specified allure-results folder does not exist: %allureResultsPath%
)