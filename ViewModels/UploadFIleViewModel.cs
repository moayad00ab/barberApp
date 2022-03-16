using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace barber.ViewModels;

    public class UploadFileViewModel
    {
       public IFormFile ImgPath { get; set; }
        public bool ifProfileImg { get; set; }
        public users User { get; set; }




    }

