using Data.Constants;
using Data.Dtos;
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


        public async Task<IActionResult> ReviewForm(int resId)
        {
            var result = User.Identity.IsAuthenticated.ToString();
            int cusId = 0;
            

            if (result == "True")
            {
                cusId = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
                bool hasBooked = await BookingApiRH.HasBooked(cusId, resId);
                if (hasBooked)
                {
                    ViewBag.cusId = cusId;
                    ViewBag.resId = resId;
                    return PartialView("_ReviewForm");
                }
            }

            return new EmptyResult();
            
        }

        public async Task<IActionResult> UpSert(tbRatingAndReview ratingNreview)
        {
           
            ResponseData result = await RatingNReviewApiRH.UpSert(ratingNreview);
            return Json(result);
        }




    }

    

}
