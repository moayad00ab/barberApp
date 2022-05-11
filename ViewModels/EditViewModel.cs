using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace barber.ViewModels;

    public class EditViewModel
    {
        [Display(Name = "ID: ")]
        public string Id { get; set; }
        [Display(Name = "Phone Number: ")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email Address: ")]
        [EmailAddress]
         [Required (ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
        [Display(Name = "Username: ")]
        public string UserName { get; set; }
        [Display(Name = "First Name: ")]
         public string fName { get; set; }
         [Display(Name = "Last Name: ")]
        public string lName { get; set; }
        [Display(Name = "Barber shop name: ")]
        public string shopName { get; set; }
        [Display(Name = "Your barber shop: ")]
        public string barbersShop { get; set; }
        [Display(Name = "District: ")]
        public string district { get; set; }
        [Display(Name = "Street: ")]
        public string street { get; set; }
        [Display(Name = "Postel code: ")]
        public int postelCode { get; set; }
        [Display(Name = "City: ")]
        public string city { get; set; }
        public float rating { get; set; }
        [Display(Name = "Start working time: ")]
        public string sWorkTime { get; set; }
        [Display(Name = "End working time: ")]
        public string eWorkTime { get; set; }
        public DateOnly daysWork { get; set; }
        public bool isAvilable { get; set; }
    }