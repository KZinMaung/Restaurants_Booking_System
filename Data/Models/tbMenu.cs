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
        public Double Price { get; set; }

        
        public string Photo { get; set; }

        [NotMapped]
        public string PhotoUrl
        {
            get
            {
                if (this.Photo != null)
                {

                    return $"https://localhost:7156/Storage/{Photo}";
                }
                else
                {
                    return $"https://localhost:7156/img/default-menu.png";

                }
            }
        }


        [NotMapped]
        public string? PhotoString { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public DateTime? Accesstime { get; set; } 

        public bool IsDeleted { get; set; } = false; 

    
    }
}
