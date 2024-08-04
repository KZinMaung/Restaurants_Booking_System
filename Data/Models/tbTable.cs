using Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace
{
    [Table("tbTable")]
    public class tbTable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        [Required]
        public int NumberOfPeople { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual tbRestaurant Restaurant { get; set; }
    }
}
