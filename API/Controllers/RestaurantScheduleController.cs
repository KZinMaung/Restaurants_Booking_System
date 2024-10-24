using API.Services.Restaurant;
using API.Services.RestaurantSchedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class RestaurantScheduleController : ControllerBase
    {
        IRestaurantSchedule _irestaurantSchedule;

        public RestaurantScheduleController(IRestaurantSchedule irestaurantSchedule)
        {
            this._irestaurantSchedule = irestaurantSchedule;
        }

        [HttpGet("api/restaurant-schedule/get-list")]
        public async Task<IActionResult> GetList(int resId)

        {
            var result = await this._irestaurantSchedule.GetList(resId);
            return Ok(result);
        }
    }
}
