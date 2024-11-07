using Data.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("tbCustomer")]
    public class tbCustomer 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Phone { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }


        public string? Photo { get; set; }

        [NotMapped]
        public string? PhotoUrl
        {
            get
            {
                if (this.Photo != null)
                {

                    return $"https://localhost:7156/Storage/{Photo}";
                }
                else
                {
                    return $"https://localhost:7156/img/user-profile.png";

                }
            }
        }

        [NotMapped]
        public string? PhotoString {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
