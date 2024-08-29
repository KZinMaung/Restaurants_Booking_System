using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel.User
{
    public class ResetPasswordRequest
    {
        public int UserID { get; set; }
        public string NewPassword { get; set; } = String.Empty;
        public string ConfirmPassword { get; set; } =  String.Empty;
    }
}
