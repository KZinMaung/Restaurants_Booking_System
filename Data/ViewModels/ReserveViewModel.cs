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
        public tbRestaurant Restaurant {  get; set; } = new tbRestaurant();
        public List<tbRestaurantSchedule> Schedules { get; set; }   = new List<tbRestaurantSchedule> { };

        public tbCustomer? Customer { get; set; }

    }
}
