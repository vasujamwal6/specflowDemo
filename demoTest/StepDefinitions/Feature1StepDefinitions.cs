using demoTest.APIActions;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using static HarmonyLib.Code;

namespace demoTest.StepDefinitions
{
    
    
    [Binding]
    public sealed class Feature1StepDefinitions
    {
        private IWebDriver driver;
        private readonly ScenarioContext _scenarioContext;
        APICalls call;
        string url = "https://www.youtube.com/";

        public Feature1StepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            _scenarioContext = scenarioContext;
            call = new APICalls();
        }


        [Given(@"Open the browser")]
        public void GivenOpenTheBrowser()
        {
            Assert.IsNotNull(driver, "WebDriver session did not initialize.");
            AddLogToContext("Launch Web Application: Youtube");
            call.addValuesToAllureReport("Web Application", "Youtube");
        }

        [When(@"Enter the URL")]
        public void WhenEnterTheURL()
        {
            driver.Url = url;
            Thread.Sleep(3000);
            AddLogToContext($"URL: {url}");
            call.addValuesToAllureReport("URL", url);
        }

        [Then(@"Search for the BBC Earth")]
        public void ThenSearchForTheBBCEarth()
        {
            driver.FindElement(By.XPath("//*[@name='search_query']")).SendKeys("BBC Earth");
            driver.FindElement(By.XPath("//*[@name='search_query']")).SendKeys(Keys.Enter);
            Thread.Sleep(5000);
            AddLogToContext("Search Term: BBC Earth");
            call.addValuesToAllureReport("Search Term", "BBC Earth");
        }

        private void AddLogToContext(string message)
        {
            var logs = _scenarioContext["StepLogs"] as List<string>;
            logs?.Add(message);
        }
    }
}
