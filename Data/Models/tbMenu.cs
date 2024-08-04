using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Table("tbMenu")]
    public class tbMenu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        public string Photo { get; set; } 

        [Required]
        [StringLength(250)]
        public string Category { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public DateTime? Accesstime { get; set; } 

        public bool IsDeleted { get; set; } = false; 

        // Navigation property
        public virtual tbRestaurant Restaurant { get; set; }
    }
}
