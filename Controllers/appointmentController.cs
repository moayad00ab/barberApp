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
    public class appointmentcontroller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<users> _userManager;


        public appointmentcontroller(ApplicationDbContext context, UserManager<users> userManager
)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View(_context.appointment.ToList());
        }
        [HttpGet]
        public async Task<ActionResult> Create(string id)
        {
            var barber = await _userManager.FindByIdAsync(id);
            var shop = await _userManager.FindByIdAsync(barber.barbersShop);
            IEnumerable<SelectListItem> serviceList = _context.services.Select(d => new SelectListItem
            {
                Value = d.description,
                Text = d.description
            });
            var model = new CreateAppointmentViewModel();
            model.service = serviceList;
            model.barberId = barber.Id;
            model.shopId = barber.barbersShop;
            model.barber = barber.fName + " " + barber.lName;
            model.shop = shop.shopName;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                appointment obj = new appointment() { Date = model.Date, barberId = model.barber, service = model.service.ToString(), User = model.User, appointApprove = model.appointApprove, shopId = model.shop };
                _context.appointment.Add(obj);
                await _context.SaveChangesAsync();
            }
            return View(model);
        }


    }
}

