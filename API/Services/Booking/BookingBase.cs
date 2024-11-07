using API.Services.EmailService;
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
        private readonly IEmailService _emailService;
        UnitOfWork _uow;


        public BookingBase(BookingSystemDbCotnext context, IEmailService _emailService)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._emailService = _emailService;
           
        }

        public async Task<int> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)
        {
            int noOfbookingTable =  await _uow.bookingRepo.GetAll().Where(b => b.RestaurantId == resId && b.RestaurantScheduleId == resScheId && b.BookingDate.Date == bookingDate.Date && b.IsDeleted != true && b.Status == BookingStatus.Confirmed).SumAsync(b => b.NoOfTable);
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
                booking.Status = BookingStatus.Confirmed;
                entity = await _uow.bookingRepo.InsertReturnAsync(booking);

                entity.BookingCode = GenerateBookingCode(entity.Id);

                // Update the booking with the generated code
                entity = await _uow.bookingRepo.UpdateAsync(entity);


                //Send confirm mail
                BookingVM data = new BookingVM();   
                data = await GetBookingInfo(entity);
                await SendConfirmEmail(data);
                
            }
           

            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }


        private async Task<BookingVM> GetBookingInfo(tbBooking booking)
        {
            BookingVM data = new BookingVM();
            data.Id = booking.Id;
            data.Booking = booking;
            data.Restaurant =await _uow.restaurantRepo.GetAll().Where(r => r.Id == booking.RestaurantId).FirstOrDefaultAsync() ?? new tbRestaurant(); 
            data.Schedule = await _uow.restaurantScheduleRepo.GetAll().Where(s => s.Id == booking.RestaurantScheduleId).FirstOrDefaultAsync() ?? new tbRestaurantSchedule();
            return data;
            
        }

        private async Task SendConfirmEmail(BookingVM data)
        {
            var formattedDate = (data.Booking.BookingDate).ToString("dd MMM yy");

            DateTime startTime = DateTime.Today.Add(data.Schedule.StartTime);
            string formattedStartTime = startTime.ToString("hh:mm tt");

            DateTime endTime = DateTime.Today.Add(data.Schedule.EndTime);
            string formattedEndTime = endTime.ToString("hh:mm tt");

            string receptor = data.Booking.CustomerEmail;
            string subject = "Your Reservation is Confirmed!";
            string body = $@"Dear {data.Booking.CustomerName},

                        Thank you for choosing {data.Restaurant.Name}!
                        We're excited to confirm your reservation on {formattedDate} at {formattedStartTime} - {formattedEndTime}.

                        Reservation Details:

                        Booking Code: {data.Booking.BookingCode}
                        Name: {data.Booking.CustomerName}
                        Time: {formattedStartTime} - {formattedEndTime}
                        Date: {formattedDate}
                        Table Count: {data.Booking.NoOfTable}

                        If you need to modify or cancel your reservation, please contact us at {data.Restaurant.Phone} or {data.Restaurant.Email}.

                        We look forward to serving you!

                        Sincerely,
                        {data.Restaurant.Name}";

            await _emailService.SendEmail(receptor, subject, body);

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


        public async Task<Model<BookingVM>> GetList(int cusId,int resId,  int page, int pageSize, string? q = "")
        {
            var currentDateTime = DateTime.Now;

            Expression<Func<BookingVM, bool>> basicFilter = null, customerFilter = null, restaurantFilter = null;
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
                                   Schedule = rs,
                                   CanCancel = b.Status == "Confirmed" &&
                                            (b.BookingDate.Date + rs.StartTime) > currentDateTime &&
                                            (b.BookingDate.Date + rs.StartTime - currentDateTime).TotalMinutes >= 30
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

            if (resId != 0)
            {
                restaurantFilter = vm => vm.Booking.RestaurantId == resId;
                query = query.Where(restaurantFilter);
            }

            var sortVal = "Id";
            var sortDir = "desc";
            query = SORTLIT<BookingVM>.Sort(query, sortVal, sortDir);
            var result = await PagingService<BookingVM>.getPaging(page, pageSize, query);
            return result;
        }


        public async Task<ResponseData> CancelBooking(int bookingId)
        {
            tbBooking booking = await _uow.bookingRepo.GetAll().Where(b => b.IsDeleted != true && b.Id == bookingId).FirstOrDefaultAsync() ?? new tbBooking();
            tbBooking? entity = null;
            ResponseData response = new ResponseData();
            if(booking.Id != 0)
            {
                booking.Status = BookingStatus.Cancelled;
                entity = await _uow.bookingRepo.UpdateAsync(booking);
            }

            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;
        }

        public async Task<ResponseData> CompleteBooking(int bookingId)
        {
            tbBooking booking = await _uow.bookingRepo.GetAll().Where(b => b.IsDeleted != true && b.Id == bookingId).FirstOrDefaultAsync() ?? new tbBooking();
            tbBooking? entity = null;
            ResponseData response = new ResponseData();
            if (booking.Id != 0)
            {
                booking.Status = BookingStatus.Completed;
                entity = await _uow.bookingRepo.UpdateAsync(booking);
            }

            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;
        }


        public bool HasBooked(int cusId, int resId)
        {
            bool hasBookings = _uow.bookingRepo.GetAll().Any(b => b.CustomerId == cusId && b.RestaurantId == resId && b.Status != BookingStatus.Cancelled && b.IsDeleted != true);
            return hasBookings;
        }
    }
}
