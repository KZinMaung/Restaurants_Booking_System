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
    }

    

}
