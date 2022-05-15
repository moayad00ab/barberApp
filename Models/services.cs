using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber.Models;

    public class services
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter service name")]
        [Display(Name = "Service name:")]
        public string name { get; set; }
        [Required(ErrorMessage = "Please enter service description")]
        [Display(Name = "Description:")]
        public string description { get; set; }
        [Required(ErrorMessage = "Please enter service price")]
        [Display(Name = "Price:")]
        public float price { get; set; }
        public string userId { get; set; }
        [Required(ErrorMessage = "Please enter expected service Duration")]
        public string time { get; set; }
        List<services> listServices { get; set; }
        public bool isChecked { get; set; }
        public float offerPrice { get; set; }
    }