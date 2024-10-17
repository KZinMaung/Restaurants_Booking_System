using Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("tbRestaurantSchedule")]
    public class tbRestaurantSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;

        
    }
}
