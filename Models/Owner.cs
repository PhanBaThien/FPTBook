using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace FPTBook_v3.Models
{
    public class Owner
    {
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
        [Required,Display(Name = "Email")]
        public string Email { get; set; }
        [Required, Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password"), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
