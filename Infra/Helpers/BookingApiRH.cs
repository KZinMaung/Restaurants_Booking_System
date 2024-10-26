using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public static class BookingApiRH
    {
        public static async Task<int> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)
        {
            var url = string.Format("api/booking/get-available-count?resId={0}&resScheId={1}&bookingDate={2}", resId, resScheId, bookingDate);
            var data = await ApiRequestBase<int>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }
    }
}
