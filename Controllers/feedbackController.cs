using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using barber.Models;
using barber;
using barber.ViewModels;
using barber.Data;
using Microsoft.AspNetCore.Identity;

namespace barber.Controllers
{
    public class feedbackcontroller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<users> _userManager;


        public feedbackcontroller(ApplicationDbContext context, UserManager<users> userManager
)
        {
            _context = context;
            _userManager = userManager;
        }
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult AddFeedback(feedbackViewModel vm){
var comment = vm.comment;
var UserId = vm.UserId;
var rating = vm.rating;
feedback comnt =new feedback(){
UserId = UserId,
comment = comment,
rating = rating,
publishDate = DateTime.Now};
_context.feedback.Add(comnt);
_context.SaveChanges();
return RedirectToAction("Details", "Account", new { id = UserId });
    }
}
}