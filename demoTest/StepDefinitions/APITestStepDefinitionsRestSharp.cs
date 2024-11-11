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
using System.Reflection.Emit;
using System.Text.Json;
using System.Net;
using Allure.Net.Commons;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;

namespace demoTest.StepDefinitions
{
    [Binding]
    public class APITestStepDefinitionsRestSharp
    {
        private readonly ScenarioContext _scenarioContext;
        APICalls call;
        string responseBody;
        string baseURI;
        string requestURI;
        private RestClient restClient;
        private RestRequest restRequest;
        private RestResponse restResponse;
        //private IWebDriver _driver;


        public APITestStepDefinitionsRestSharp(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            call = new APICalls();
            baseURI = Hooks.Hooks.config.BaseURI;
            restClient = new RestClient(baseURI);
            //this._driver = driver;
        }


        [Given(@"User has an ID (.*) to request for (.*)")]
        public void GivenUserHasAnID(int id, string endpoint)
        {
            requestURI = baseURI + endpoint + "/" + id;
            restRequest = new RestRequest(endpoint + "/" + id, Method.Get);
            AddLogToContext($"[User ID: {id}]");
            call.addValuesToAllureReport("ID", Convert.ToString(id));
            AddLogToContext("[GET] [Request URI: " + requestURI + "]");
            call.addValuesToAllureReport("Request URI", requestURI);

        }

        [When(@"User requests details using endpoint and ID (.*)")]
        public async Task WhenUserRequestsDetailsUsingObjectsAndID(int id)
        {
            restResponse = restClient.Execute(restRequest);
            responseBody = restResponse.Content.ToString();
            AddLogToContext("[Response Status: " + (int)restResponse.StatusCode + "]");
            AddLogToContext("[Response Body: " + responseBody + "]");
            call.addValuesToAllureReport("Response Status", restResponse.StatusCode.ToString());
            call.addValuesToAllureReport("Response Body", responseBody);

        }

        [Then(@"User should recieve object details with name as (.*) and status code (.*)")]
        public void ThenUserShouldRecieveObjectWithNameAndStatusCode(string name, int statusCode)
        {
            var content = Utility.HandleContent.GetContent<demoAPIModel>(restResponse);
            Assert.AreEqual(statusCode, (int)restResponse.StatusCode, "Received Incorrect Status Code");
            Assert.AreEqual(name, content.name.ToString(), "Incorrect name value found in response");
            AddLogToContext("[Response Verified]: " + responseBody);
            call.addValuesToAllureReport("Verified Response", responseBody);

        }

        [Given(@"User wants to create new object with parameters as (.*) and (.*)")]
        public void user_wants_to_create_new_object_with_params(string attribute1, int attribute2)
        {
            AddLogToContext("[Color = " + attribute1 + " & Capacity = " + attribute2 + "]");
            call.addValuesToAllureReport("Color", attribute1);
            call.addValuesToAllureReport("Capacity", Convert.ToString(attribute2));
            requestURI = baseURI + "objects";
            restRequest = new RestRequest(requestURI, Method.Post);
            AddLogToContext("[Post] [Request URI: " + requestURI + "]");
            call.addValuesToAllureReport("Request URI", requestURI);

        }

        [When(@"User creates new object with endpoint (.*) having (.*) and (.*)")]
        public async Task user_creates_newobjectAsync(string endpoint, string attribute1, int attribute2)
        {
           
            var contentData = call.returnJsonBody(attribute1, attribute2);
            call.addValuesToAllureReport("Request Body", contentData.ToString());
            restRequest.AddJsonBody(contentData);
            restResponse = restClient.Execute(restRequest);
            responseBody = restResponse.Content.ToString();
            AddLogToContext("[Response Body: " + responseBody + "]");
            call.addValuesToAllureReport("Response Body", responseBody);
        }

        [Then(@"User verifies new object is created with color (.*), capacity GB and status (.*)")]
        public void user_verifies_newobject_is_created(string color, int statusCode)
        {
            var content = Utility.HandleContent.GetContent<demoAPIModel>(restResponse);
            Assert.AreEqual(statusCode, (int)restResponse.StatusCode, "Received Incorrect Status Code");
            Assert.AreEqual(color, content.data.color.ToString(), "Incorrect color value found in response");
            AddLogToContext("[Response Verified]: " + responseBody);
            call.addValuesToAllureReport("Verified Response", responseBody);
        }

        private void AddLogToContext(string message)
        {
            var logs = _scenarioContext["StepLogs"] as List<string>;
            logs?.Add(message);
        }
    }
}
