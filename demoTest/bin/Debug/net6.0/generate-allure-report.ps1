# Define paths for the allure-results and allure-report directories
$allureResultsPath = "C:\Users\vasudev.singh\source\repos\demoTest\demoTest\bin\Debug\net6.0\allure-results"
$allureReportPath = "C:\Users\vasudev.singh\source\repos\demoTest\demoTest\bin\Debug\net6.0\allure-report"

# Check if the allure-results directory exists
if (Test-Path $allureResultsPath) {
    Write-Host "Generating Allure Report from: $allureResultsPath"
    
    # Generate the Allure report
    allure generate $allureResultsPath --clean -o $allureReportPath

    if ($?) {
        Write-Host "Allure report generated successfully. Opening report..."
        allure open $allureReportPath
        
        # Wait for the report to be opened before clearing old results
        Start-Sleep -Seconds 5  # Adjust the wait time as needed
        
        # Clear old Allure results after report generation
        Write-Host "Clearing old Allure results at: $allureResultsPath"
        Remove-Item -Recurse -Force "$allureResultsPath\*"
        Write-Host "Old Allure results cleared."
    } else {
        Write-Host "Failed to generate the Allure report."
    }
} else {
    Write-Host "The specified allure-results folder does not exist: $allureResultsPath"
}
