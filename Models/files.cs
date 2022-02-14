using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace barber.Models;

    public class files
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int file_id { get; set; }
        public string ImgPath { get; set; }
        public bool ifProfileImg { get; set; }
        public users User { get; set; }
    }
