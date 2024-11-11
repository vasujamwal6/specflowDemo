### SpecFlow Selenium C# BDD Automation Framework

### Configuration: 
> API Automation frameworks: RestSharp & HTTPClient
> Reporting: Allure reports & Extent reports


1. To generate allure reports manually run 'generate-allure-report.batch' file under BatchFiles folder

2. To automatically generate allure report everytime, enable batch file call in [AfterTestRun] function under Hooks.cs 

3. Extent reports are available under TestResults folder

4. To perform parallel execution update 'LevelOfParallelism' value under AssemblyInfo.cs file