using Data.Models;

namespace API.Services.Booking
{
    public interface IBooking
    {
        Task<int> GetAvailableCount(int resId, int resScheId, DateTime bookingDate);
    }
}
