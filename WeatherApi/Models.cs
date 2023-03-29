using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApi
{
    public class BookingDeatils
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int totalprice { get; set; }
        public bool depositpaid { get; set; }
        public Bookingdates bookingdates { get; set; }
        public string additionalneeds { get; set; }
    }

    public class Bookingdates
    {
        public string checkin { get; set; }
        public string checkout { get; set; }
    }

    public class ResponseBookingModel
    {
        public int bookingid { get; set; }
        public BookingDeatils booking { get; set; }
    }


    public class ResponseGetAllBookingsModel
    {
        public int bookingid { get; set; }
    }

}
