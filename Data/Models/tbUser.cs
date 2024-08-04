using Data.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("tbUser")]
    public class tbUser
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

        [Required]
        [StringLength(250)]
        public string Role { get; set; } = Roles.User;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? Accesstime { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
