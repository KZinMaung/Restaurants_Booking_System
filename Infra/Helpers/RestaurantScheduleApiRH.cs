using Data.Dtos;
using Data.Models;
using Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public static class RestaurantScheduleApiRH
    {
        public static async Task<List<tbRestaurantSchedule>> GetList(int resId)
        {
            string url = string.Format("api/restaurant-schedule/get-list?resId={0}", resId);
            var data = await ApiRequestBase<List<tbRestaurantSchedule>>.GetRequest(url.route(Request.BookingSystem));
            return data;

        }
    }
}
