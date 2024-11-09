using API.Services.Customer;
using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.Helpers;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.Restaurant
{
    public class RestaurantBase:IRestaurant
    {
        private readonly BookingSystemDbCotnext _context;
        private readonly IConfiguration _configuration;
        private IAzureBlobStorage _azureBlobStorage;
        UnitOfWork _uow;
       

        public RestaurantBase(BookingSystemDbCotnext context, IAzureBlobStorage azureBlobStorage, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            this._azureBlobStorage = azureBlobStorage;
            
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private async Task InsertSchedules(tbRestaurant restaurant)
        {
            // Generate the restaurant schedule
            TimeSpan openTime = restaurant.OpenTime;
            TimeSpan closedTime = restaurant.CloseTime;
            int durationInMinutes = restaurant.Duration;

            List<tbRestaurantSchedule> schedules = new List<tbRestaurantSchedule>();

            for (TimeSpan current = openTime; current < closedTime; current = current.Add(TimeSpan.FromMinutes(durationInMinutes)))
            {
                var schedule = new tbRestaurantSchedule
                {
                    RestaurantId = restaurant.Id,
                    StartTime = current,
                    EndTime = current.Add(TimeSpan.FromMinutes(durationInMinutes)),
                    CreatedAt = DateTime.Now,
                    Accesstime = DateTime.Now
                };


                // Ensure the schedule doesn't go beyond the closing time
                if (schedule.EndTime > closedTime)
                {
                    schedule.EndTime = closedTime;
                }

                schedules.Add(schedule);
            }

            // Insert schedules into the database
            await _uow.restaurantScheduleRepo.InsertListAsync(schedules);
        }

        public int DeleteSchedules(int resId)
        {
            var schedules = _uow.restaurantScheduleRepo.GetAll().Where(rs => rs.IsDeleted != true && rs.RestaurantId == resId).ToList();
            var result = _uow.restaurantScheduleRepo.DeleteList(schedules);
            return result;

        }

        private async Task UpdateSchedules(tbRestaurant restaurant)
        {
            DeleteSchedules(restaurant.Id);
            await InsertSchedules(restaurant);

        }
        public async Task<ResponseData> UpSert(tbRestaurant restaurant)
        {
            tbRestaurant entity;
            ResponseData response = new ResponseData();
            //update
            if (restaurant.Id != 0)
            {

                restaurant.Accesstime = MyExtension.GetLocalTime();

                var oldEntity = await _uow.restaurantRepo.GetAll().Where(r => r.IsDeleted != true && r.Id == restaurant.Id).AsNoTracking().FirstOrDefaultAsync() ?? new tbRestaurant();
                if (oldEntity.OpenTime != restaurant.OpenTime || oldEntity.CloseTime != restaurant.CloseTime || oldEntity.Duration != restaurant.Duration)
                {
                    entity = await _uow.restaurantRepo.UpdateAsync(restaurant);
                    await UpdateSchedules(entity);
                }
                else
                {
                    entity = await _uow.restaurantRepo.UpdateAsync(restaurant);
                }

            }
            //insert
            else
            {
                restaurant.Password = HashPassword(restaurant.Password);
                restaurant.Accesstime = MyExtension.GetLocalTime();
                restaurant.CreatedAt = MyExtension.GetLocalTime();
                entity = await _uow.restaurantRepo.InsertReturnAsync(restaurant);

                if (entity != null)
                {
                     await InsertSchedules(entity);
                }


            }


            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }


        public async Task<tbRestaurant> GetById(int id)
        {
            return await _uow.restaurantRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbRestaurant();
        }



        public async Task<Model<RestaurantWithRating>> GetTopRatedRestaurants(int page, int pageSize, string? q = "")
        {
            string? sortVal = "AverageRating";
            string? sortDir = "desc";
            // Join Restaurant with RatingAndReview and group the results by restaurant
            var query = from r in _uow.restaurantRepo.GetAll().Where(r => r.IsDeleted != true)
                        join rr in _uow.ratingNReviewRepo.GetAll() on r.Id equals rr.RestaurantId into ratingsGroup
                        select new RestaurantWithRating
                        {
                            Restaurant = r,
                            AverageRating = ratingsGroup.Any() ? ratingsGroup.Average(rr => rr.Rating) : 0
                        };

            // Apply search filter
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(r => r.Restaurant.Name.Contains(q));
            }

            query = SORTLIT<RestaurantWithRating>.Sort(query, sortVal, sortDir);
            // Paging the result
            var pagedResult = await PagingService<RestaurantWithRating>.getPaging(page, pageSize, query);
            return pagedResult;
        }


        public async Task<Model<RestaurantWithRating>> GetList(int page, int pageSize, string? sortVal = "Id", string? sortDir = "desc",
                        string? q = "")
        {
            Expression<Func<tbRestaurant, bool>> basicFilter = null;

            var query = from r in _uow.restaurantRepo.GetAll().Where(r => r.IsDeleted != true)
                        join rr in _uow.ratingNReviewRepo.GetAll() on r.Id equals rr.RestaurantId into ratingsGroup
                        select new RestaurantWithRating
                        {
                            Restaurant = r,
                            AverageRating = ratingsGroup.Any() ? Math.Round(ratingsGroup.Average(rr => rr.Rating), 1) : 0
                        };

            // Apply search filter
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(r => r.Restaurant.Name.Contains(q));
            }


            // Apply sorting (default is by AverageRating in descending order)
            if (sortDir == "asc")
            {
                query = query.OrderBy(r => EF.Property<object>(r.Restaurant, sortVal));
            }
            else
            {
                query = query.OrderByDescending(r => EF.Property<object>(r.Restaurant, sortVal));
            }

            var result = await PagingService<RestaurantWithRating>.getPaging(page, pageSize, query);
            return result;
        }


    }
}
