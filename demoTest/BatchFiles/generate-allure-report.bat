@echo off
setlocal
echo Running Allure Report Generation...
powershell -ExecutionPolicy Bypass -File generate-allure-report.ps1
:: Check the exit code of the PowerShell script
if errorlevel 1 (
    echo PowerShell script exited with an error. Halting execution.
    exit /b 1
)

:: Proceed with additional batch file logic if PowerShell script was successful
echo PowerShell script executed successfully. Continuing...
endlocal