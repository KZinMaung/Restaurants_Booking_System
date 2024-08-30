using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class tbBookingTable
    {
        public int Id { get; set; }
        public int BookingId {  get; set; }
        public int TableId {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? Accesstime { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
