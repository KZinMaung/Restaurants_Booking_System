using Data.Dtos;
using Data.Models;
using Data.ViewModels;
using Infra.Services;

namespace API.Services.Booking
{
    public interface IBooking
    {
        Task<int> GetAvailableCount(int resId, int resScheId, DateTime bookingDate);

        Task<ResponseData> UpSert(tbBooking booking);

        Task<Model<BookingVM>> GetList(int cusId,int resId, int page, int pageSize, string? q = "");


    }
}
