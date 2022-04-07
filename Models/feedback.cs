using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace barber.Models;

    public class feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string UserId { get; set; }
        public users User { get; set; }
        public string comment { get; set; }
        public DateTime publishDate { get; set; }
        public int rating { get; set; }
    }
