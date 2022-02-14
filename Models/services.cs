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
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Price")]
        public float price { get; set; }
        public users User { get; set; }

    }
