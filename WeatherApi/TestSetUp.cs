using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.ApiClient;
using TestProject.Utility;

namespace BookingApi
{
    [SetUpFixture]
    public class OneTimeExecution : BaseTest
    {
        /// <summary>
        /// Below method intialize the report generation
        /// and One Time Token for the Test Run
        /// </summary>
        /// <returns></returns>
        [OneTimeSetUp]
        public async Task oneTimeSetup()
        {
            ExtentReportGen.InitializeReport();
            

                var myHttpClient = new HttpClient();
            // For Sample Test The values are hardcoded 
                var payload = "{\"username\":\"admin\",\"password\":\"password123\"}";

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
            var authUrl = ApiRequestHandler.BaseUri + "auth";
                var response = await myHttpClient.PostAsync(authUrl, content);
                var token = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestHandler.Token = token.GetValue("token").ToString();
                }
                else
                {
                ExtentReportGen.FailMessage(ExtentReportGen.extentTest, "TOKEN_NOT_GENERATED:");
                    ApiRequestHandler.Token = "#TOKEN_NOT_GENERATED#";
                }
            
        }

        [OneTimeTearDown]
        public void extentReportCleanup()
        {
            try
            {
                ExtentReportGen.extentReports.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class TestSetUp : BaseTest
    {
      
        [SetUp]
        public Task Init()
        {
            ExtentReportGen.extentTest = ExtentReportGen.extentReports.CreateTest(TestContext.CurrentContext.Test.Name, "Test ID : " + TestContext.CurrentContext.Test.ID);
            return Task.CompletedTask;
        }

        [TearDown]
        public Task TestTearDown()
        {
            try
            {
                //Validating test with pass/fail/skipped for report log
                var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
                var errorMessage = TestContext.CurrentContext.Result.Message;
                switch (testStatus)
                {
                    case TestStatus.Failed:
                        ExtentReportGen.FailMessage(ExtentReportGen.extentTest, "Test Failed with " + errorMessage);
                        break;
                    case TestStatus.Skipped:
                        ExtentReportGen.SkipMessage(ExtentReportGen.extentTest, "Test Skipped with " + errorMessage);
                        break;
                    case TestStatus.Warning:
                        ExtentReportGen.WarnMessage(ExtentReportGen.extentTest, errorMessage);
                        break;
                    default:
                        ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Test ended with Pass");
                        break;
                }
            }
            catch (Exception ex)
            {
                ExtentReportGen.FailMessage(ExtentReportGen.extentTest, "Some Unknown Error is thrown " + ex.StackTrace);
                Assert.Fail($"Some Unknown Error is thrown :: " + ex);
            }

            return Task.CompletedTask;
        }
    }
}
