using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class ReserveViewModel
    {
        public tbRestaurant restaurant {  get; set; } = new tbRestaurant();
        public List<tbRestaurantSchedule> schedules { get; set; }   = new List<tbRestaurantSchedule> { };

        public tbCustomer? customer { get; set; }

    }
}
