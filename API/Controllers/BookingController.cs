using API.Services.Booking;
using Data.Models;
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

        [HttpPost("api/booking/upsert")]
        public async Task<IActionResult> UpSert(tbBooking booking)
        {
            var result = await this._ibooking.UpSert(booking);
            return Ok(result);
        }


        [HttpGet("api/booking/get-list")]
        public async Task<IActionResult> GetList(int cusId = 0,int resId = 0, int page = 1, int pageSize = 10, string? q = "")

        {
            var result = await this._ibooking.GetList(cusId, resId, page, pageSize, q);
            return Ok(result);
        }
    }
}
