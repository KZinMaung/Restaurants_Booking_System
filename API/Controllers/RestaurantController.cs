using API.Services.Customer;
using API.Services.Restaurant;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        IRestaurant _irestaurant;
        
        public RestaurantController(IRestaurant irestaurant)
        {
            this._irestaurant = irestaurant;
        }

        [HttpPost("api/restaurant/upsert")]
        public async Task<IActionResult> UpSert(tbRestaurant restaurant)
        {
            var result = await this._irestaurant.UpSert(restaurant);
            return Ok(result);
        }

        [HttpGet("api/restaurant/get-by-id")]
        public async Task<IActionResult> GetById(int id)

        {
            var result = await this._irestaurant.GetById(id);
            return Ok(result);
        }


        [HttpGet("api/restaurant/get-top-rated-restaurants")]
        public async Task<IActionResult> GetTopRatedRestaurants(int page, int pageSize, string? q = "")

        {
            var result = await this._irestaurant.GetTopRatedRestaurants(page,pageSize,q);
            return Ok(result);
        }

        [HttpGet("api/restaurant/get-list")]
        public async Task<IActionResult> GetList(int page = 1, int pageSize = 10, string? sortVal = "Id", string? sortDir = "desc",
                                string? q = "")

        {
            var result = await this._irestaurant.GetList(page, pageSize, sortVal, sortDir, q);
            return Ok(result);
        }

    }
}
