using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ApiClient
{
    public class TestApiClient : ApiRequestHandler
    {

        public TestApiClient() => BookingUri = new Uri(BaseUri);



        #region Booking


        public async Task<HttpResponseMessage> Create_Booking(string userRequest, HttpStatusCode resultCode = HttpStatusCode.OK)
        {
            var json = JObject.Parse(userRequest);
            var response = await SendRequest<JObject>(
                Method.Post,
                string.Format($"booking"),
                new[] { resultCode }, json);

            return response;
        }

        public async Task<HttpResponseMessage> Get_Bookings()
        {
            
            var response = await SendRequest(
                Method.Get,
                "booking",
                new[] { HttpStatusCode.OK });
            return response;
        }

        public async Task<HttpResponseMessage> Get_Booking_By_ID(int id)
        {

            var response = await SendRequest(
                Method.Get,String.Format($"booking/{id}"),
                new[] { HttpStatusCode.OK });
            return response;
        }

        #endregion



    }
}
