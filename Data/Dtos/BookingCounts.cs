using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class BookingCounts
    {
        public int AllCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
        public int ConfirmedCount { get; set; }
    }
}
