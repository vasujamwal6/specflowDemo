using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using demoTest.Utility;
using demoTest.Configuration;
using Microsoft.Extensions.Configuration;
using AventStack.ExtentReports.Configuration;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using Allure.Net.Commons;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using demoTest.Pages;

namespace demoTest.Hooks
{
    [Binding]
    public sealed class Hooks : ExtentReport
    {
        private readonly IObjectContainer _container;
        public static ConfigSetting config;
        static IWebDriver driver;
        private const string WinAppDriverUrl = "http://127.0.0.1:4723";
        private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        private static WindowsDriver<WindowsElement> desktopDriver;

        static string configsettingpath = System.IO.Directory.GetParent(@"../../../").FullName
            + Path.DirectorySeparatorChar + "Configuration/configsetting.json";

        static string clearResultsBatchFilePath = System.IO.Directory.GetParent(@"../../../").FullName
            + Path.DirectorySeparatorChar + "BatchFiles\\clear-allure-results.bat";

        static string generateResultsBatchFilePath = System.IO.Directory.GetParent(@"../../../").FullName
            + Path.DirectorySeparatorChar + "BatchFiles\\generate-allure-report.bat";
        public Hooks(IObjectContainer container)
        {
            _container = container;

        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Common.RunBatchFile(clearResultsBatchFilePath);
            config = new ConfigSetting();
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(configsettingpath);
            IConfigurationRoot configuration = builder.Build();
            configuration.Bind(config);
            Console.WriteLine("Running before test run...");
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("Running after test run...");
            ExtentReportTearDown();
            //RunBatchFile(generateResultsBatchFilePath);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            Console.WriteLine("Running before feature...");
            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("Running after feature...");
        }

        [BeforeScenario("@Testers")]
        public void BeforeScenarioWithTag()
        {
            Console.WriteLine("Running inside tagged hooks in specflow");
        }

        [BeforeScenario("@Calculator")]
        public static void Setup()
        {
            if (driver == null)
            {
                var appOptions = new AppiumOptions();

                appOptions.AddAdditionalCapability("app", CalculatorAppId);

                desktopDriver = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), appOptions);
            }
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running before scenario...");
            if (scenarioContext.ScenarioInfo.Tags.Contains("UI"))
            {
                //driver = GetWebDriver();                                   //For new version configuration
                driver = GetWebDriver1(); 

                _container.RegisterInstanceAs<IWebDriver>(driver);
            }
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            scenarioContext["StepLogs"] = new List<string>(); // Initialize a log list for the scenario

        }

        public IWebDriver GetWebDriver1()
        {
            if (Hooks.config.BrowserType.ToLower() == "chrome")
            {
                new DriverManager().SetUpDriver(new ChromeConfig());
                driver = new ChromeDriver();
            }
            else if (Hooks.config.BrowserType.ToLower() == "firefox")
            {
                new DriverManager().SetUpDriver(new FirefoxConfig());
                driver = new FirefoxDriver();
            }
            else if (Hooks.config.BrowserType.ToLower() == "edge")
            {
                new DriverManager().SetUpDriver(new EdgeConfig());
                driver = new EdgeDriver();
            }
            driver?.Manage().Window.Maximize();

            return driver;
        }

        public IWebDriver GetWebDriver() {
            if (Hooks.config.BrowserType.ToLower() == "chrome")
            {
                driver = new ChromeDriver();
            }
            else if (Hooks.config.BrowserType.ToLower() == "firefox")
            {
                driver = new FirefoxDriver();
            }
            else if (Hooks.config.BrowserType.ToLower() == "edge")
            {
                driver = new EdgeDriver();
            }
            driver?.Manage().Window.Maximize();

            return driver;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("Running after scenario...");
            //var driver = _container.Resolve<IWebDriver>();
            if (driver != null) {
                driver.Quit();
                driver.Dispose();
            } 
       
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running after step....");
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            var stepLogs = scenarioContext["StepLogs"] as List<string>;

            //var driver = _container.Resolve<IWebDriver>();
            //When scenario passed
            if (scenarioContext.TestError == null)
            {
                ExtentTest stepNode = stepType switch
                {
                    "Given" => _scenario.CreateNode<Given>(stepName),
                    "When" => _scenario.CreateNode<When>(stepName),
                    "Then" => _scenario.CreateNode<Then>(stepName),
                    _ => _scenario.CreateNode<And>(stepName)
                };

                // Log each stored message to the step node
                if (stepLogs != null)
                {
                    foreach (var log in stepLogs)
                    {
                        stepNode.Info(log);
                    }
                }
            }

            //When scenario fails
            if (scenarioContext.TestError != null)
            {
                if (driver != null)
                {
                    if (stepType == "Given")
                    {
                        _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message,
                            MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build()).Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "When")
                    {
                        _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message,
                            MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build()).Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "Then")
                    {
                        _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message,
                            MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build()).Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "And")
                    {
                        _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message,
                            MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenarioContext)).Build()).Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    byte[] content = Common.CaptureScreenshot(driver);
                    AllureApi.AddAttachment("Failed Screenshot", "image/png", content);
                }
                else {
                    if (stepType == "Given")
                    {
                        _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message)
                          .Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "When")
                    {
                        _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message)
                          .Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "Then")
                    {
                        _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message)
                          .Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }
                    else if (stepType == "And")
                    {
                        _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message)
                          .Log(AventStack.ExtentReports.Status.Fail, scenarioContext.TestError.StackTrace);
                    }


                }
               
            }
            scenarioContext["StepLogs"] = new List<string>();

        }

        [AfterScenario("@Calculator")]
        public static void TearDown()
        {
            desktopDriver?.Quit();
            desktopDriver = null;
        }

        public static WindowsDriver<WindowsElement> GetDriver() => desktopDriver;

      
    }
}