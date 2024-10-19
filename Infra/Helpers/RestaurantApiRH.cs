﻿using Data.Dtos;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public class RestaurantApiRH
    {
        public static async Task<ResponseData> UpSert(tbRestaurant restaurant)
        {

            string url = string.Format("api/restaurant/upsert");
            ResponseData data = await ApiRequestBase<tbRestaurant,ResponseData>.PostDiffRequest(url.route(Request.BookingSystem), restaurant);
            return data;
        }
    }
}
