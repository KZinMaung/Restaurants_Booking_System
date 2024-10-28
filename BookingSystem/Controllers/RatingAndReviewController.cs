using Data.Models;
using Infra.Helpers;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class RatingAndReviewController : Controller
    {
        public async Task<IActionResult> GetList(int resId = 0, int page = 1, int pageSize = 10, string? q = "")
        {

            PagedListClient<tbMenu> result = await MenuApiRH.GetList(resId, page, pageSize, q);

            return PartialView("_MenuList", result);
        }
    }
}
