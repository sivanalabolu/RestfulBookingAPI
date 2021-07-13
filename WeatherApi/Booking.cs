using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.ApiClient;
using TestProject.Utility;

namespace BookingApi
{
    [TestFixture]    
    public class Booking : TestSetUp
    {

        [Test]
        [Category("SmokeTest")]
        [Category("Regression")]
        public async Task Create_A_Booking()
        {

            ExtentReportGen.LogMessage(ExtentReportGen.extentTest, "Test for creating a booking ");
            try
            {
                var body = "{\"firstname\":\"siva\",\"lastname\":\"Brown\",\"totalprice\":111,\"depositpaid\":true,\"bookingdates\":{\"checkin\":\"2018-01-01\",\"checkout\":\"2019-01-01\"},\"additionalneeds\":\"Breakfast\"}";
                var response = await ApiClient.Create_Booking(body);
              //  var booking = await response.Content.ReadAsStringAsync();
                var bookingResp = JsonConvert.DeserializeObject<ResponseBookingModel>(response.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(bookingResp.Bookingid);
                Assert.IsNotNull(bookingResp.bookingModel.Firstname);
                Assert.IsNotNull(bookingResp.bookingModel.Lastname);
                ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Booking Created as :" + bookingResp);
            }
            catch (Exception e)
            {
                ExtentReportGen.PrintException(ExtentReportGen.extentTest, e);
                Assert.Fail(e.StackTrace);
            }
        }

        [Test, TestCase("1", TestName = "Get All Bookings"), Category("SmokeTest")]
        public async Task Get_All_Bookings(string TestName)
        {

            ExtentReportGen.LogMessage(ExtentReportGen.extentTest,  "Test for :: "+ TestName);
            try
            {
                var response = await ApiClient.Get_Bookings();
                JArray jArray = JsonConvert.DeserializeObject<JArray>(response.Content.ReadAsStringAsync().Result);
               var GetAllBookinsResponse = JsonConvert.DeserializeObject<List<Bookingids>>(jArray.ToString());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(GetAllBookinsResponse[0].Bookingid, "Id Is Null");

                });
                    ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Booking Details displayed are :" + GetAllBookinsResponse);
            }
            catch (Exception e)
            {
                ExtentReportGen.PrintException(ExtentReportGen.extentTest, e);
                Assert.Fail(e.StackTrace);
            }
        }

        [Test, TestCase(2, TestName = "Get Booking By id"), Category("SmokeTest")]
        public async Task Get_BookingBy_ID(int id )
        {

            ExtentReportGen.LogMessage(ExtentReportGen.extentTest, "Test started" );
            try
            {
                var response = await ApiClient.Get_Booking_By_ID(id);
                var BookingIdResponse = await response.Content.ReadAsStringAsync();

                ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Booking Details displayed are :" + BookingIdResponse);
            }
            catch (Exception e)
            {
                ExtentReportGen.PrintException(ExtentReportGen.extentTest, e);
                Assert.Fail(e.StackTrace);
            }
        }


    }
}
