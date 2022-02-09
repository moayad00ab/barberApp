using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber;

    public class offers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string offerID { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Expire date")]
        public DateTime expire { get; set; }
        [Display(Name = "Barber shop")]
        public users User { get; set; }

    }
