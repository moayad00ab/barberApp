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
    [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
    [Display(Name = "Customer")]
    public users User { get; set; }
    public string shopId { get; set; }
    [Display(Name = "Barber shop: ")]
    public string shop { get; set; }
    [Display(Name = "Approval")]
    public bool appointApprove { get; set; }
    [Display(Name = "Service")]
    [Required(ErrorMessage = "please select service")]
    public IEnumerable<SelectListItem> service { get; set; }
    public string description {get; set;}
    public float price {get; set;}



}