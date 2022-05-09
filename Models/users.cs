using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace barber;

    public class users : IdentityUser
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public string shopName { get; set; }
        public string barbersShop { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public int postelCode { get; set; }
        public string city { get; set; }
        public float rating { get; set; }
        public string sWorkTime { get; set; }
        public string eWorkTime { get; set; }
        [NotMapped]
        public string [] daysWork { get; set; }
        public bool isAvilable { get; set; }

    }

