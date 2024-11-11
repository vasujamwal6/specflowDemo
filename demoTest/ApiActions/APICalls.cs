using Allure.Net.Commons;
using demoTest.APIModels;
using demoTest.StepDefinitions;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace demoTest.APIActions
{
    public class APICalls
    {
        private IWebDriver driver;
        HttpClient client;
        HttpResponseMessage response;
        public APICalls()
        {
            client = new HttpClient();
            //this.driver = driver;
        }


        public async Task<HttpResponseMessage> getResponseUsingID(string requestURI) {
            Console.WriteLine(requestURI);
            response = await client.GetAsync(requestURI);
            return response;
        }

        public async Task<HttpResponseMessage> createData(string requestURI, StringContent body) {
            Console.WriteLine(requestURI);
            response = await client.PostAsync(requestURI, body);
            return response;
        }
        public StringContent returnRestBody(string color, int capacity) {
            var contentdata = new StringContent("{\"name\":\"Test\",\"data\": {\"color\":\"" + color + "\", \"capacityGB\":\"" + capacity + "\"}}", Encoding.UTF8, "application/json");
            return contentdata;
        }

        public object returnJsonBody(string color, int capacity)
        {
            var contentData = new
            {
                name = "Test",
                data = new
                {
                    color = color,
                    capacity = capacity
                }
            };
            return contentData;
        }

        public Boolean verifyResponseContainsValue(string responseBody , string name) {
            Console.WriteLine("ResponseBody: " + responseBody);
            var data = JsonConvert.DeserializeObject<demoAPIModel>(responseBody);
            return name.Equals(data.name.ToString());
        }

        public string returnResponseAttributes(string responseBody)
        {
            Console.WriteLine("ResponseBody: " + responseBody);
            var data = JsonConvert.DeserializeObject<demoAPIModel>(responseBody);
            return data.data.color;
        }

        public void addValuesToAllureReport(string name, string value) {
            AllureLifecycle.Instance.UpdateStep(stepResult =>
            {
                stepResult.parameters.Add(
                    new Allure.Net.Commons.Parameter { name = name, value = value }
                );
            });
        }
    }
}
