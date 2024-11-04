using Data.Models;
using Data.ViewModels;
using Infra.Helpers;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class RatingAndReviewController : Controller
    {
        public async Task<IActionResult> GetList(int resId = 0, int page = 1, int pageSize = 10)
        {
            PagedListClient<RatingAndReviewVM> result = await RatingNReviewApiRH.GetList(resId, page, pageSize);
            return PartialView("_ReviewList", result);
        }
    }
}
