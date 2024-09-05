﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("tbBooking")]
    public class tbBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int RestaurantScheduleId { get; set; }


        [Required]
        public int NumberOfPeople { get; set; }

        public string? Status { get; set; } 

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserPhone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public DateTime? Accesstime { get; set; } 

        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public List<int> TableIds { get; set; } = new List<int>();
        
    }
}
