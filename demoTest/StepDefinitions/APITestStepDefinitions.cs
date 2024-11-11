using demoTest.APIModels;
using demoTest.Hooks;
using demoTest.APIActions;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SpecFlow.Internal.Json;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using RestSharp;

namespace demoTest.StepDefinitions
{
    [Binding]
    public class APITestStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        APICalls call;
        HttpResponseMessage response;
        string responseBody;
        string baseURI;
        string requestURI;
        //private IWebDriver _driver;


        public APITestStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            call = new APICalls();
            baseURI = Hooks.Hooks.config.BaseURI;
            //this._driver = driver;

        }

        [Given(@"User has a parameter (.*)")]
        public void GivenUserHasAParameter(int id)
        {
            AddLogToContext($"[User ID: {id}]");
            call.addValuesToAllureReport("ID", Convert.ToString(id));

        }

        [When(@"User requests object details using (.*) and (.*)")]
        public async Task WhenUserRequestsObjectDetailsUsingObjectsAndAsync(string endpoint, int id)
        {
            requestURI = baseURI + endpoint + "/"+ id;
            AddLogToContext("[GET] [Request URI: " + requestURI+"]");
            call.addValuesToAllureReport("Request URI", requestURI);
            response = await call.getResponseUsingID(requestURI);
            responseBody = await response.Content.ReadAsStringAsync();
        }

        [Then(@"User should get object with name as (.*) and status code (.*)")]
        public void ThenUserShouldGetObjectWithNameAndStatusCode(string name, int statusCode)
        {
            AddLogToContext("[Respose Status: " + (int)response.StatusCode + "]");
            AddLogToContext("[Respose Body: " + responseBody + "]");
            call.addValuesToAllureReport("Response Status", response.StatusCode.ToString());
            call.addValuesToAllureReport("Response Body", responseBody);
            Assert.AreEqual(statusCode, ((int)response.StatusCode), "Received Incorrect Status Code: " + ((int)response.StatusCode));
            Assert.IsTrue(call.verifyResponseContainsValue(responseBody, name), "Incorrect name value found in response");
        }

        [Given(@"User has new object parameters as (.*) and (.*)")]
        public void user_wants_to_create_new_object(string attribute1, int attribute2)
        {
            AddLogToContext("[Color = " + attribute1 + " & Capacity = " + attribute2+"]");
            call.addValuesToAllureReport("Color", attribute1);
            call.addValuesToAllureReport("Capacity", Convert.ToString(attribute2));
        }

        [When(@"User creates object with endpoint (.*) having (.*) and (.*)")]
        public async Task user_creates_objectAsync(string endpoint, string attribute1, int attribute2)
        {
            requestURI = baseURI + endpoint;
            AddLogToContext("[Post] [Request URI: " + requestURI + "]");
            var contentData = call.returnRestBody(attribute1, attribute2);
            AddLogToContext("[Request Body: " + contentData.ReadAsStringAsync().Result + "]");
            call.addValuesToAllureReport("Request URI", requestURI);
            call.addValuesToAllureReport("Request Body", contentData.ReadAsStringAsync().Result);

            response = await call.createData(requestURI, contentData);
            responseBody = await response.Content.ReadAsStringAsync();
        }

        [Then(@"User verifies object is created with color (.*), capacity GB and status (.*)")]
        public void user_verifies_object_is_created(string color, int statusCode)
        {
            AddLogToContext("[Respose Status: " + (int)response.StatusCode + "]");
            AddLogToContext("[Respose Body: " + responseBody + "]");
            call.addValuesToAllureReport("Response Status", response.StatusCode.ToString());
            call.addValuesToAllureReport("Response Body", responseBody);
            Assert.AreEqual(statusCode, ((int)response.StatusCode), "Received Incorrect Status Code: " + ((int)response.StatusCode));
            string dataValue = call.returnResponseAttributes(responseBody);
            Assert.IsTrue(dataValue.Contains(color), "Incorrect color value found in response" + dataValue);
        }

        private void AddLogToContext(string message)
        {
            var logs = _scenarioContext["StepLogs"] as List<string>;
            logs?.Add(message);
        }
    }
}
