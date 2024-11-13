### SpecFlow Selenium C# BDD Automation Framework

### Configuration: 
> API Automation frameworks: RestSharp & HTTPClient
> Reporting: Allure reports & Extent reports

### Pre-Requisite for Desktop Automation:
1. Install 'WinAppDriver.exe' before execution <https://github.com/microsoft/WinAppDriver/releases/tag/v1.2.1>
2. Open Command prmt and enter below commands
>cd C:\Program Files (x86)\Windows Application Driver
>WinAppDriver.exe

### Points:
1. To generate allure reports manually run 'generate-allure-report.batch' file under BatchFiles folder

2. To automatically generate allure report everytime, enable batch file call in [AfterTestRun] function under Hooks.cs 

3. Extent reports are available under TestResults folder

4. To perform parallel execution update 'LevelOfParallelism' value under AssemblyInfo.cs file