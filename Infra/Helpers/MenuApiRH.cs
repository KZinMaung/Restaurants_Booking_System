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
    public static class MenuApiRH
    {
        public static async Task<ResponseData> UpSert(tbMenu menu)
        {

            string url = string.Format("api/menu/upsert");
            ResponseData data = await ApiRequestBase<tbMenu, ResponseData>.PostDiffRequest(url.route(Request.BookingSystem), menu);
            return data;
        }

        public static async Task<PagedListClient<tbMenu>> GetList(int resId = 0, int page = 1, int pageSize = 10, string? q = "")
        {
            string url = string.Format("api/menu/get-list?resId={0}&page={1}&pageSize={2}&q={3}", resId, page, pageSize, WebUtility.UrlEncode(q));
            var data = await ApiRequestBase<PagedListServer<tbMenu>>.GetRequest(url.route(Request.BookingSystem));
            PagedListClient<tbMenu> model = PagingService<tbMenu>.ConvertFromPagedListServer(page, pageSize, data);
            return model;


        }

        public static async Task<tbMenu> GetById(int id)
        {
            var url = string.Format("api/menu/get-by-id?id={0}", id);
            var data = await ApiRequestBase<tbMenu>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }


        public static async Task<ResponseData> Delete(int id)
        {
            string url = string.Format("api/menu/delete?id=" + id);
            var data = await ApiRequestBase<ResponseData>.GetRequest(url.route(Request.BookingSystem));
            return data;
        }
    }
}
