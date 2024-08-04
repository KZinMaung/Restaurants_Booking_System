using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("tbRestaurant")]
    public class tbRestaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public string Location { get; set; }

        public string Division { get; set; }

        public string Township { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string CoverPhoto { get; set; }

        public string ProfilePhoto { get; set; }

        public DateTime? OpenTime { get; set; }

        public DateTime? CloseTime { get; set; }

        public int? Duration { get; set; }

        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual tbUser User { get; set; } // Assuming you have a User entity
    }
}
