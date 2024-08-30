using Core.Interfaces;
using Data.Model;
using Data.Models;
using Infra.Repository;
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
        private BookingSystemDbContext _ctx;
        private IRepository<tbUser> _userRepo;
        private IRepository<tbBooking> _bookingRepo;
        private IRepository<tbBookingTable> _bookingTableRepo;
        public UnitOfWork(BookingSystemDbContext ctx)
        {
            _ctx = ctx;
        }

        public UnitOfWork()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
            var contextOptions = new DbContextOptionsBuilder<BookingSystemDbContext>()
              .UseSqlServer(configuration.GetConnectionString("ConnectionStrings"))
              .Options;
            _ctx = new BookingSystemDbContext(contextOptions);

        }
        ~UnitOfWork()
        {
            _ctx.Dispose();
        }

        public IRepository<tbUser> userRepo
        {
            get
            {
                if(_userRepo == null)
                {
                    _userRepo = new Repository<tbUser>(_ctx);
                }
                return _userRepo;
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

        public IRepository<tbBookingTable> bookingTableRepo
        {
            get
            {
                if (_bookingTableRepo == null)
                {
                    _bookingTableRepo = new Repository<tbBookingTable>(_ctx);
                }
                return _bookingTableRepo;
            }
        }
    }

    

}
