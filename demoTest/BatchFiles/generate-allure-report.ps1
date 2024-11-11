# Define paths for the allure-results and allure-report directories
$allureResultsPath = "C:\Users\vasudev.singh\source\repos\demoTest\demoTest\bin\Debug\net6.0\allure-results"
$allureReportPath = "C:\Users\vasudev.singh\source\repos\demoTest\demoTest\bin\Debug\net6.0\allure-report"

# Check if the allure-results directory exists
if (Test-Path $allureResultsPath) {
    Write-Host "Clearing old Allure report at: $allureReportPath"
    if (Test-Path $allureReportPath) {
        Remove-Item -Recurse -Force $allureReportPath
    }

    Write-Host "Generating new Allure Report from: $allureResultsPath"
    allure generate $allureResultsPath --clean -o $allureReportPath

    if ($?) {
        Write-Host "Allure report generated successfully. Opening report..."
        allure open $allureReportPath
		Write-Host "Exiting execution."
        exit 1
    } else {
        Write-Host "Failed to generate the Allure report."
    }
} else {
    Write-Host "The specified allure-results folder does not exist: $allureResultsPath"
}

Write-Host "Exiting execution."
exit 1