using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class ResetPasswordVM
    {
        public ResetPasswordVM() { }
        public ResetPasswordVM(string token,string email)
        {
            Token = token;
            Email = email;
        }
        public string Token { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
