using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using barber.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace barber.ViewModels;

public class CreateAppointmentViewModel
{
    public string barberId { get; set; }
    [Display(Name = "Barber: ")]
    public string barber { get; set; }
    [Display(Name = "Date")]
    [Required(ErrorMessage = "please choose date")]
    [BindProperty, DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly Date { get; set; }
    [Display(Name = "Customer")]
    public users User { get; set; }
    public string shopId { get; set; }
    [Display(Name = "Barber shop: ")]
    public string shop { get; set; }
    [Display(Name = "Approval")]
    public bool appointApprove { get; set; }
    [Display(Name = "Service")]
    public IList<SelectListItem> services { get; set; }
    [Required(ErrorMessage = "please select service")]
    public string service {get; set;}
    public string stime {get; set;}



}