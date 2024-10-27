using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Data.ViewModels;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<ResponseData> UpSert(tbBooking booking)
        {
            tbBooking entity;
            ResponseData response = new ResponseData();
            //update
            if (booking.Id != 0)
            {

                booking.Accesstime = MyExtension.GetLocalTime();
                entity = await _uow.bookingRepo.UpdateAsync(booking);
            }
            //insert
            else
            {

                booking.Accesstime = MyExtension.GetLocalTime();
                booking.CreatedAt = MyExtension.GetLocalTime();
                booking.Status = BookingStatus.Pending;
                entity = await _uow.bookingRepo.InsertReturnAsync(booking);

                entity.BookingCode = GenerateBookingCode(entity.Id);

                // Update the booking with the generated code
                entity = await _uow.bookingRepo.UpdateAsync(entity);
            }


            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }


        private static string GenerateBookingCode(int bookingId)
        {
            // Prefix for identification
            string prefix = "BKG";

            // Format current date as YYYYMMDD
            string datePart = DateTime.Now.ToString("yyyyMMdd");

            // Use the booking ID or any unique identifier
            string uniquePart = bookingId.ToString("D5"); // Pads to 5 digits, e.g., 00001, 00002

            // Combine parts to create the booking code
            string bookingCode = $"{prefix}-{datePart}-{uniquePart}";

            return bookingCode;
        }


        public async Task<Model<BookingVM>> GetList(int cusId, int page, int pageSize, string? q = "")
        {
            Expression<Func<BookingVM, bool>> basicFilter = null, customerFilter = null;
            var bookings = _uow.bookingRepo.GetAll().Where(b => b.IsDeleted != true).AsQueryable();
            var restaurants = _uow.restaurantRepo.GetAll().Where(r => r.IsDeleted != true).AsQueryable();
            var restaurantSchedules = _uow.restaurantScheduleRepo.GetAll().Where(rs => rs.IsDeleted != true).AsQueryable();

            var query = (from b in bookings
                               join r in restaurants on b.RestaurantId equals r.Id
                               join rs in restaurantSchedules on b.RestaurantScheduleId equals rs.Id
                               select new BookingVM
                               {
                                   Id = b.Id,
                                   Booking = b,
                                   Restaurant = r,
                                   Schedule = rs
                               }).AsQueryable();


            if (!string.IsNullOrEmpty(q))
            {
                basicFilter = vm => vm.Booking.BookingCode.Contains(q);
                query = query.Where(basicFilter);
            }

            if (cusId != 0)
            {
                customerFilter = vm => vm.Booking.CustomerId == cusId;
                query = query.Where(customerFilter);
            }

            var sortVal = "Id";
            var sortDir = "desc";
            query = SORTLIT<BookingVM>.Sort(query, sortVal, sortDir);
            var result = await PagingService<BookingVM>.getPaging(page, pageSize, query);
            return result;
        }

    }
}
