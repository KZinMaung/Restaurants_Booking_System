using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.Services;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.RestaurantSchedule
{
    public class RestaurantScheduleBase : IRestaurantSchedule
    {
        private readonly BookingSystemDbCotnext _context;
        UnitOfWork _uow;


        public RestaurantScheduleBase(BookingSystemDbCotnext context)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
       
        }

        public async Task<List<tbRestaurantSchedule>> GetList(int resId)
        {

            IQueryable<tbRestaurantSchedule> query = _uow.restaurantScheduleRepo.GetAll().Where(a => a.IsDeleted != true && a.RestaurantId == resId).AsQueryable();
            return await query.ToListAsync();
        }

    }
}
