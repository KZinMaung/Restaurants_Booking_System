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
    public static class RatingNReviewApiRH
    {
        public static async Task<PagedListClient<RatingAndReviewVM>> GetList(int resId = 0, int page = 1, int pageSize = 10)
        {
            string url = string.Format("api/rating-and-review/get-list?resId={0}&page={1}&pageSize={2}", resId, page, pageSize);
            var data = await ApiRequestBase<PagedListServer<RatingAndReviewVM>>.GetRequest(url.route(Request.BookingSystem));
            PagedListClient<RatingAndReviewVM> model = PagingService<RatingAndReviewVM>.ConvertFromPagedListServer(page, pageSize, data);
            return model;

        }

        public static async Task<ResponseData> UpSert(tbRatingAndReview ratingNreview)
        {

            string url = string.Format("api/rating-and-review/upsert");
            ResponseData data = await ApiRequestBase<tbRatingAndReview, ResponseData>.PostDiffRequest(url.route(Request.BookingSystem), ratingNreview);
            return data;
        }
    }
}
