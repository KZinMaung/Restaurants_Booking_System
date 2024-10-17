using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Constants
{
    public static class UserType
    {
        public const string Customer = "customer";
        public const string Restaurant = "restaurant";
    }

    public static class BookingStatus
    {
        public const string Booked = "booked";
    }

    public static class ResponseStatus
    {
        public const string Success = "success";
        public const string Fail = "fail";
    }

    public struct AuthDataIndex
    {
        public const int Id = 0;
        public const int Name = 1;
        public const int Email = 2;
        public const int Phone = 3;
    }
}
