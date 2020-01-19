using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using empty_project.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace empty_project.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote("IsEmailValid", "Account")]
        [ValidEmailDomain("facebook", "apple", "vog", "google", "microsoft", ErrorMessage = "Invalid domain name, must be 'apple', 'facebook', 'google', 'vog', or 'microsoft'.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
