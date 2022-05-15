using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using barber.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace barber.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First name: ")]
        public string fName { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last name: ")]
        public string lName { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number: ")]
        [DataType(DataType.PhoneNumber)]

        public String PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email Address: ")]
        [EmailAddress]
        public String Email { get; set; }

        [EmailAddress]
        [Display(Name = "Confirm Email: ")]
        [Compare("Email", ErrorMessage = "Email and confirmation email do not match.")]
        public String EmailConfirmed { get; set; }
        [Required(ErrorMessage = "Please enter your username")]
        [Display(Name = "Username: ")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password: ")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public String PConfirmation { get; set; }
        [Display(Name = "Your shop name: ")]
        public string shop { get; set; }
        public IEnumerable<SelectListItem> shops { get; set; }
        [Display(Name = "Choose your shop: ")]
        public string barbersShop { get; set; }
        [Required(ErrorMessage = "Chose your account type")]
        public string userType { get; set; }
    }
}
