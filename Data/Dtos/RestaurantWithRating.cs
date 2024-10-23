using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class RestaurantWithRating
    {
        public tbRestaurant Restaurant { get; set; }
        public double AverageRating { get; set; }
    }

}
