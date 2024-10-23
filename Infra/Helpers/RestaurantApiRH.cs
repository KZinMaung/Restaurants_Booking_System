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
    public class RestaurantApiRH
    {
        public static async Task<ResponseData> UpSert(tbRestaurant restaurant)
        {

            string url = string.Format("api/restaurant/upsert");
            ResponseData data = await ApiRequestBase<tbRestaurant,ResponseData>.PostDiffRequest(url.route(Request.BookingSystem), restaurant);
            return data;
        }

        public static async Task<tbRestaurant> GetById(int id)
        {
            var url = string.Format("api/restaurant/get-by-id?id={0}", id);
            var data = await ApiRequestBase<tbRestaurant>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }

        public static async Task<PagedListClient<RestaurantWithRating>> GetTopRatedRestaurants(int page = 1, int pageSize = 10, string? q = "")
        {
            string url = string.Format("api/restaurant/get-top-rated-restaurants?page={0}&pageSize={1}&q={2}", page, pageSize, WebUtility.UrlEncode(q));
            var data = await ApiRequestBase<PagedListServer<RestaurantWithRating>>.GetRequest(url.route(Request.BookingSystem));
            PagedListClient<RestaurantWithRating> model = PagingService<RestaurantWithRating>.ConvertFromPagedListServer(page, pageSize, data);
            return model;
        }

        public static async Task<PagedListClient<RestaurantWithRating>> GetList(int page = 1, int pageSize = 10, string? q = "")
        {
            string url = string.Format("api/restaurant/get-list?page={0}&pageSize={1}&q={2}", page, pageSize, WebUtility.UrlEncode(q));
            var data = await ApiRequestBase<PagedListServer<RestaurantWithRating>>.GetRequest(url.route(Request.BookingSystem));
            PagedListClient<RestaurantWithRating> model = PagingService<RestaurantWithRating>.ConvertFromPagedListServer(page, pageSize, data);
            return model;

        }
    }
}
