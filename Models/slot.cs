using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace barber.Models;

    public class slot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public users User { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
