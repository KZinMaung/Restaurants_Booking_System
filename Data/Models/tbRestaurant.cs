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
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Division { get; set; }
        [Required]
        public string Township { get; set; }


        public string? Quote { get; set; }


        public string? Description { get; set; }

        public string? CoverPhoto { get; set; }

        [NotMapped]
        public string? CoverPhotoString { get; set; }

        public string? ProfilePhoto { get; set; }

        [NotMapped]
        public string? ProfilePhotoString { get; set; }

        [Required]
        public DateTime OpenTime { get; set; }

        [Required]
        public DateTime CloseTime { get; set; }

        [Required]
        public int? Duration { get; set; }

        [Required]
        public int? NoOfTable { get; set; }

        [Required]
        public int? PeoplePerTable { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
