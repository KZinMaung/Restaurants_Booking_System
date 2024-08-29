using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Phone { get; set; }
        [Required]
        [RegularExpression("^(user|owner)$")]
        public string? Role { get; set; }
    }
}
