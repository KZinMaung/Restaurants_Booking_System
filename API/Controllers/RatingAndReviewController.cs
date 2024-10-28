using API.Services.Menu;
using API.Services.RatingAndReview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class RatingAndReviewController : ControllerBase
    {
        IRatingAndReview _iratingNreview;
        public RatingAndReviewController(IRatingAndReview iratingNreview)
        {
            this._iratingNreview = iratingNreview;
        }

        [HttpGet("api/rating-and-review/get-list")]
        public async Task<IActionResult> GetList(int resId = 0, int page = 1, int pageSize = 10)

        {
            var result = await this._iratingNreview.GetList(resId, page, pageSize);
            return Ok(result);
        }

    }
}
