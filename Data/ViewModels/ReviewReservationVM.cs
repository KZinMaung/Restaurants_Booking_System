using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class ReviewReservationVM
    {
        public string RestaurantName { get; set; } = string.Empty;
        public string BookingDate { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public int NoOfTable {  get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
