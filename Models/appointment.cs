using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using barber.Models;
using Microsoft.AspNetCore.Mvc;

namespace barber;

public class appointment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string appointID { get; set; }
    [Display(Name = "Barber")]
    public string barberId { get; set; }
    [Display(Name = "Date")]
    [Required(ErrorMessage = "please choose date")]
    [BindProperty, DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public string Date { get; set; }
    [Display(Name = "Customer")]
    public users User { get; set; }
    [Display(Name = "Barber shop")]
    public string shopId { get; set; }
    [Display(Name = "Approval")]
    public bool appointApprove { get; set; }
    [Display(Name = "Service")]
    [Required(ErrorMessage = "please select service")]
    public List<services> service { get; set; }
    [Display(Name = "Payment")]
    public bool payStatus { get; set; }
    public string stime { get; set; }
    public string etime { get; set; }


}
