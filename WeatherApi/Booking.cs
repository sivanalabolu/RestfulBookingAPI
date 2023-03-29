using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                var bookingResp = JsonConvert.DeserializeObject<ResponseBookingModel>(response.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(bookingResp.bookingid);
                Assert.IsNotNull(bookingResp.booking.firstname);
                Assert.IsNotNull(bookingResp.booking.lastname);
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
                var GetAllBookinsResponse = JsonConvert.DeserializeObject<List<ResponseGetAllBookingsModel>>(jArray.ToString());
                Assert.Multiple(() =>
                {
                    Assert.NotNull(GetAllBookinsResponse[0].bookingid, "Id Is Null");

                });
                ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Booking Details displayed are :" + GetAllBookinsResponse);
            }
            catch (Exception e)
            {
                ExtentReportGen.PrintException(ExtentReportGen.extentTest, e);
                Assert.Fail(e.StackTrace);
            }
        }

        [Test, TestCase(TestName = "Get Booking By id"), Category("SmokeTest")]
        public async Task Get_BookingBy_ID()
        {
            Random rnd = new Random();
            int id  =rnd.Next(1, 999);

            ExtentReportGen.LogMessage(ExtentReportGen.extentTest, "Test started" );
            try
            {
                var response = await ApiClient.Get_Booking_By_ID(id);
                var bookingByIdResp = JsonConvert.DeserializeObject<BookingDeatils>(response.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(bookingByIdResp.firstname);
                Assert.IsNotNull(bookingByIdResp.lastname);
                Assert.IsNotNull(bookingByIdResp.depositpaid);
                Assert.IsNotNull(bookingByIdResp.totalprice);
                Assert.IsNotNull(bookingByIdResp.additionalneeds);
                Assert.IsNotNull(bookingByIdResp.bookingdates.checkin);
                Assert.IsNotNull(bookingByIdResp.bookingdates.checkout);

                ExtentReportGen.PassMessage(ExtentReportGen.extentTest, "Booking Details displayed are :" + bookingByIdResp);
            }
            catch (Exception e)
            {
                ExtentReportGen.PrintException(ExtentReportGen.extentTest, e);
                Assert.Fail(e.StackTrace);
            }
        }


    }
}
