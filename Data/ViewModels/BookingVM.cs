using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class BookingVM
    {
        public int Id { get; set; }
        public tbBooking Booking { get; set; }
        public tbRestaurant Restaurant { get; set; }
        public tbRestaurantSchedule Schedule { get; set; }
        public bool CanCancel { get; set; }
    }
}
