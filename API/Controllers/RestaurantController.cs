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


    }
}
