using Data.Model;
using Data.Models;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Booking
{
    public class BookingBase : IBooking
    {
        private readonly BookingSystemDbCotnext _context;
        UnitOfWork _uow;


        public BookingBase(BookingSystemDbCotnext context)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
           
        }

        public async Task<int> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)
        {
            int noOfbookingTable =  await _uow.bookingRepo.GetAll().Where(b => b.RestaurantId == resId && b.RestaurantScheduleId == resScheId && b.BookingDate.Date == bookingDate.Date && b.IsDeleted != true).SumAsync(b => b.NoOfTable);
            tbRestaurant res = await _uow.restaurantRepo.GetAll().Where(r => r.Id == resId && r.IsDeleted != true).FirstOrDefaultAsync() ?? new tbRestaurant();
            int availableCount = res.NoOfTable - noOfbookingTable;
            return availableCount;
        }
    }
}
