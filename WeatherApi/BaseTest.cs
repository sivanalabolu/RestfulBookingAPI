using Microsoft.Extensions.Configuration;
using System;
using TestProject.ApiClient;

namespace BookingApi
{
    public class BaseTest
    {
        public TestApiClient ApiClient = new TestApiClient();

        public static string environmentType { get; set; } = string.Empty;

        public static IConfiguration Configuration;

        //username and password 
        public static string username { get; set; }
        public static string password { get; set; }

        static BaseTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();


            environmentType = Configuration["TestEnvironmentType"];

            #region - Test Case Inputs
            ApiRequestHandler.BaseUri = Configuration[$"{environmentType}:uri"];
            switch (environmentType)
            {
                case "QA":
                    username = "admin";
                    password = "password123";
                    break;

                default:
                    Console.WriteLine("Given username and password is not valid for above environment");
                    break;
            }
            #endregion
        }

    }
}