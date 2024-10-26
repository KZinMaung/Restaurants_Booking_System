using API.Services.Booking;
using API.Services.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class BookingController : ControllerBase
    {
        IBooking _ibooking;
        public BookingController(IBooking ibooking)
        {
            this._ibooking = ibooking;
        }

        [HttpGet("api/booking/get-available-count")]
        public async Task<IActionResult> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)

        {
            var result = await this._ibooking.GetAvailableCount(resId, resScheId, bookingDate);
            return Ok(result);
        }
    }
}
