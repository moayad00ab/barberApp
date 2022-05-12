using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace barber.ViewModels
{
    public class insightsViewModel
    {
        [Display(Name = "Number of all appointments ")]
        public string numOfAllAppointment { get; set; }
        [Display(Name = "Number of appointments for the last 30 days")]
        public string numOfAppointmentsLast30 { get; set; }

        [Display(Name = "Remember me")]
        public List<appointment> appointInfo { get; set; }

        
    }


}