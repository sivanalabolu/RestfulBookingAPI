using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestProject.Utility;

namespace TestProject.ApiClient
{
    public abstract class ApiRequestHandler 
    {

        public static string Token { get; set; }

        public static string BaseUri { get; set; }
        protected Uri BookingUri { get; set; }

        protected HttpClient ApiClient = new HttpClient();
        
       
        public enum Method
        {
            Get = 0,
            Post = 1,
            Put = 2,
            Delete = 3

        }

        /// <summary>
        /// Method for passing the parameters for API without body
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="successStatusCodes"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendRequest(Method method, string uri,
            IEnumerable<HttpStatusCode> successStatusCodes)
        {
            return await SendRequest<object>(method, uri, successStatusCodes, null);
        }
        /// <summary>
        /// Method for passing the parameters for API with body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="successStatusCodes"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendRequest<T>(Method method, string uri, IEnumerable<HttpStatusCode> successStatusCodes, T body = null, string clientResource = null) where T : class
        {

            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (uri == string.Empty) throw new ArgumentException("Parameter \"uri\" cannot be empty string");
            if (successStatusCodes == null) throw new ArgumentNullException(nameof(successStatusCodes));
            var successStatusCodesList = successStatusCodes.ToList();
            if (!successStatusCodesList.Any()) throw new ArgumentException("Parameter \"successStatusCodes\" must have at least one member inside it.");

          

            var response = await GetResponseAsync(method, uri, body);

            if (!successStatusCodesList.Contains(response.StatusCode))
            {
                Console.WriteLine("Expected:" + successStatusCodesList[0] + "\nWas:" + response.StatusCode);
                var rawResponseBody = response.Content.ReadAsStringAsync().Result;
                string Api = BaseUri + uri;
                var HttpMethod = method.ToString();
                var JsonBody = string.Empty;

                if (body != null)
                {
                    JsonBody = JsonConvert.SerializeObject(body);
                }
                else
                {
                    JsonBody = string.Empty;
                }

                try
                {
                    var str = response.Content.ReadAsStringAsync().Result;

                    var error = JsonConvert.DeserializeObject<JObject>(str);
                   
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            return response;
        }

        /// <summary>
        /// Generic method for calling all HTTP verbs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetResponseAsync<T>(Method method, string uri, T body)
        {
            var json = new JsonSerializerSettings();
            json.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            json.NullValueHandling = NullValueHandling.Ignore;
            var formatter = new JsonMediaTypeFormatter { SerializerSettings = json };

            HttpResponseMessage response;
          
           
            using (ApiClient = new HttpClient(CreateHandler()) { BaseAddress = BookingUri })
            {
                    ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);



                ExtentReportGen.LogMessage(ExtentReportGen.extentTest, $"Full URL is : " + method + " --" + ApiClient.BaseAddress + uri);
                ExtentReportGen.LogMessage(ExtentReportGen.extentTest, $"Request Payload: {JsonConvert.SerializeObject(body)}");
                switch (method)
                {
                    case Method.Get:
                        response = await ApiClient.GetAsync(uri);
                        break;
                    case Method.Post:
                        if (body != null)
                        {
                            response = await ApiClient.PostAsync(uri, body, formatter);
                        }
                        else
                        {
                            response = await ApiClient.PostAsync(uri, new StringContent(string.Empty));
                        }
                        break;
                    case Method.Put:
                        if (body != null)
                        {
                            response = await ApiClient.PutAsync(uri, body, formatter);
                        }
                        else
                        {
                            response = await ApiClient.PutAsync(uri, new StringContent(string.Empty));
                        }
                        break;
                    case Method.Delete:
                        if (body != null)
                        {
                            response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, uri) { Content = new ObjectContent<T>(body, new JsonMediaTypeFormatter()) });
                        }
                        else
                        {
                            response = await ApiClient.DeleteAsync(uri);
                        }
                        break;
                    default:
                        ExtentReportGen.LogMessage(ExtentReportGen.extentTest, $"Argument : " + method + " --" + "Not In the List");
                        throw new ArgumentOutOfRangeException("method");
                }
            }



            ApiClient.DefaultRequestHeaders.Authorization = null;
            ExtentReportGen.LogMessage(ExtentReportGen.extentTest,
                $"API " + "Status Code -- " + $"{response.StatusCode} " + " -- Response Body -- " +
                $"{response.Content.ReadAsStringAsync().Result}");
            return response;

        }


        private HttpClientHandler CreateHandler()
        {
            var clientHandler = new HttpClientHandler();

            clientHandler.CookieContainer = new CookieContainer();

            return clientHandler;
        }
    }
}
