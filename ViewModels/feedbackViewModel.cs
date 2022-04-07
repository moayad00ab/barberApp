using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using barber.Models;

namespace barber.ViewModels
{
    public class feedbackViewModel
    {


         public string UserId { get; set; }
        public List<feedback> comments { get; set; }
         public string User { get; set; }
        public string comment { get; set; }
        public int rating { get; set; }
    }


}


