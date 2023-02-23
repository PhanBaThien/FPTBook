using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace FPTBook_v3.Models
{
    public class ChangePassword
    {
        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password), Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password), Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Required, DataType(DataType.Password), Display(Name = "Confirm New Password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
