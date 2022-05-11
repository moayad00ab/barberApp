using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using barber.Models;

namespace barber.ViewModels
{
    public class ProfileViewModel
    {


          public string Id { get; set; }

        public string ShopName { get; set; }

        public string startTime { get; set; }
        public string EndTime { get; set; }
        public List<users> Barbers { get; set; }
        public string ProfileImg { get; set; }
        public List<files> Imgs { get; set; }

        public List<services> ListServices { get; set; }
        public bool IsAvilable { get; set; }
   

    }


}


