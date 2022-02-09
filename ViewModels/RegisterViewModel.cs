using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Phone Number: ")]
        [DataType(DataType.PhoneNumber)]
        
        public String PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email Address: ")]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Confirm Email: ")]
        [Compare("Email", ErrorMessage = "Email and confirmation email do not match.")]
        public String EmailConfirmed { get; set; }
        [Required]
        [Display(Name = "Username: ")]
        public String UserName { get; set; }
        [Required]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password: ")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public String PConfirmation { get; set; }
    }
}
