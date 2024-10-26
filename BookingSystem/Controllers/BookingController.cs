using Data.Models;
using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class BookingController : Controller
    {
        public async Task<IActionResult> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)
        {
            int count = await BookingApiRH.GetAvailableCount(resId, resScheId, bookingDate);
            return Ok(count);
        }
    }
}
