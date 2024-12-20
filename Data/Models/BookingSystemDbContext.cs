﻿
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
using System.Text;
using System.Threading.Tasks;



namespace Data.Model
{
    public class BookingSystemDbCotnext : DbContext
    {
        public BookingSystemDbCotnext(DbContextOptions<BookingSystemDbCotnext> options) : base(options)
        {
            
        }
        
        public virtual DbSet<tbBooking> tbBooking { get; set; }
        public virtual DbSet<tbMenu> tbMenu { get; set; }
        public virtual DbSet<tbRatingAndReview> tbRatingAndReview { get; set; }
        public virtual DbSet<tbRestaurant> tbRestaurant { get; set; }
        public virtual DbSet<tbRestaurantSchedule> tbRestaurantAndSchedule { get; set; }
        public virtual DbSet<tbCustomer> tbCustomer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

      
    }
}
