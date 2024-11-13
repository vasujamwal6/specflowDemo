using demoTest.APIActions;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.Code;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace demoTest.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private readonly WindowsDriver<WindowsElement> _driver;
        private readonly ScenarioContext _scenarioContext;
        APICalls call;
        string responseBody;
        string baseURI;
        string requestURI;
        private RestClient restClient;
        private RestRequest restRequest;
        private RestResponse restResponse;
        //private IWebDriver _driver;


        public CalculatorStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            call = new APICalls();
            _driver = Hooks.Hooks.GetDriver();
        }

        [Given(@"the Calculator application is open")]
        public void GivenTheCalculatorApplicationIsOpen()
        {
            Assert.IsNotNull(_driver, "Calculator application did not launch.");
            AddLogToContext("Launch App: Calculator");
            call.addValuesToAllureReport("Application", "Calculator");
        }

        [When(@"I enter ""([^""]*)""")]
        public void WhenIEnterAnd(string number)
        {
            _driver.FindElementByName(number).Click();
            AddLogToContext($"Enter Number: {number}");
            call.addValuesToAllureReport("Number", number);
        }

        [When(@"I press the ""([^""]*)"" button")]
        public void WhenIPressTheButton(string buttonName)
        {
            _driver.FindElementByName(buttonName).Click();
            AddLogToContext($"Click Button: {buttonName}");
            call.addValuesToAllureReport("Click Button", buttonName);
        }

        [Then(@"the result should be ""([^""]*)""")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
            var result = _driver.FindElementByAccessibilityId("CalculatorResults").Text;
            Assert.AreEqual("Display is " + expectedResult, result, $"Expected result: {expectedResult}, but got: {result}");
            AddLogToContext($"Result: {result}");
            call.addValuesToAllureReport("Result", result);
        }

        private void AddLogToContext(string message)
        {
            var logs = _scenarioContext["StepLogs"] as List<string>;
            logs?.Add(message);
        }
    }
}

