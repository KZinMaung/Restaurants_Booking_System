using Data.Dtos;
using Data.Models;
using Data.ViewModels;
using Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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


        public static async Task<ResponseData> UpSert(tbBooking booking)
        {
            string url = string.Format("api/booking/upsert");
            ResponseData data = await ApiRequestBase<tbBooking, ResponseData>.PostDiffRequest(url.route(Request.BookingSystem), booking);
            return data;
        }

        public static async Task<PagedListClient<BookingVM>> GetList(int cusId = 0,int resId = 0, int page = 1, int pageSize = 10, string? q = "")
        {
            string url = string.Format("api/booking/get-list?cusId={0}&resId={1}&page={2}&pageSize={3}&q={4}", cusId, resId, page, pageSize, WebUtility.UrlEncode(q));
            var data = await ApiRequestBase<PagedListServer<BookingVM>>.GetRequest(url.route(Request.BookingSystem));
            PagedListClient<BookingVM> model = PagingService<BookingVM>.ConvertFromPagedListServer(page, pageSize, data);
            return model;

        }

        public static async Task<ResponseData> CancelBooking(int bookingId)
        {
            var url = string.Format("api/booking/cancel-booking?bookingId={0}", bookingId);
            var data = await ApiRequestBase<ResponseData>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }

        public static async Task<ResponseData> CompleteBooking(int bookingId)
        {
            var url = string.Format("api/booking/complete-booking?bookingId={0}", bookingId);
            var data = await ApiRequestBase<ResponseData>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }

    }
}
