using Core.Interfaces;
using Data.Model;
using Data.Models;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.UnitOfWork
{
    public class UnitOfWork
    {
        private BookingSystemDbCotnext _ctx;
        private IRepository<tbCustomer> _customerRepo;
        private IRepository<tbBooking> _bookingRepo;
        private IRepository<tbMenu> _menuRepo;
        private IRepository<tbRatingAndReview> _ratingNReviewRepo;
        private IRepository<tbRestaurant> _restaurantRepo;
        private IRepository<tbRestaurantSchedule> _restaurantScheduleRepo;

        public UnitOfWork(BookingSystemDbCotnext ctx)
        {
            _ctx = ctx;
        }

        public UnitOfWork()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
            var contextOptions = new DbContextOptionsBuilder<BookingSystemDbCotnext>()
              .UseSqlServer(configuration.GetConnectionString("ConnectionStrings"))
              .Options;
            _ctx = new BookingSystemDbCotnext(contextOptions);

        }
        ~UnitOfWork()
        {
            _ctx.Dispose();
        }

        public IRepository<tbCustomer> customerRepo
        {
            get
            {
                if(_customerRepo == null)
                {
                    _customerRepo = new Repository<tbCustomer>(_ctx);
                }
                return _customerRepo;
            }
        }

        public IRepository<tbBooking> bookingRepo
        {
            get
            {
                if (_bookingRepo == null)
                {
                    _bookingRepo = new Repository<tbBooking>(_ctx);
                }
                return _bookingRepo;
            }
        }

        public IRepository<tbMenu> menuRepo
        {
            get
            {
                if (_menuRepo == null)
                {
                    _menuRepo = new Repository<tbMenu>(_ctx);
                }
                return _menuRepo;
            }
        }


        public IRepository<tbRatingAndReview> ratingNReviewRepo
        {
            get
            {
                if (_ratingNReviewRepo == null)
                {
                    _ratingNReviewRepo = new Repository<tbRatingAndReview>(_ctx);
                }
                return _ratingNReviewRepo;
            }
        }


        public IRepository<tbRestaurant> restaurantRepo
        {
            get
            {
                if (_restaurantRepo == null)
                {
                    _restaurantRepo = new Repository<tbRestaurant>(_ctx);
                }
                return _restaurantRepo;
            }
        }

        public IRepository<tbRestaurantSchedule> restaurantScheduleRepo
        {
            get
            {
                if (_restaurantScheduleRepo == null)
                {
                    _restaurantScheduleRepo = new Repository<tbRestaurantSchedule>(_ctx);
                }
                return _restaurantScheduleRepo;
            }
        }
    }

    

}
