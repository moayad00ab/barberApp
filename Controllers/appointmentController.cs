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
using System.Text;

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
        public async Task<ActionResult> Index()
        {
             System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var id = _userManager.GetUserId(User); // Get user id:
                var user = await _userManager.FindByIdAsync(id);
                var appointments = _context.appointment.Where(a => a.barberId == user.Id || a.User.Id == user.Id);
            return View(appointments.ToList());
        }
        [HttpGet]
        public async Task<ActionResult> Create(string id)
        {
            var barber = await _userManager.FindByIdAsync(id);
            var shop = await _userManager.FindByIdAsync(barber.barbersShop);
            List<services> serviceList = _context.services.Where(s => s.userId == shop.Id).Select(d => new services
            {  
                name = d.name,
                price = d.price,
                isChecked = false,
                time = d.time
            }).ToList();
            var today = DateOnly.FromDateTime(DateTime.Now).ToString();
            List<appointment> tempAppointment = _context.appointment.Where(a => a.Date == today).ToList();
            
            List<timeList> barberSlots = _context.timeList.Where(a => a.barber.Id == barber.Id).ToList();
            for (int j = 0; j < tempAppointment.Count; j++)
            {
                
            
            for (int i = 0; i < barberSlots.Count; i++)
            {
                if (barberSlots[i].strtime == tempAppointment[j].stime)
                {
                   barberSlots.Remove(barberSlots.Find(x => x.strtime == barberSlots[i].strtime)); 
                }
            }
            }
            ViewBag.timeList = new SelectList(barberSlots, nameof(timeList.id), nameof(timeList.strtime));
             System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var Id = _userManager.GetUserId(User); // Get user id:
            var model = new CreateAppointmentViewModel();
            model.services = serviceList;
            model.barberId = barber.Id;
            model.shopId = barber.barbersShop;
            model.barber = barber.fName + " " + barber.lName;
            model.shop = shop.shopName;
            model.customerId = Id;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateAppointmentViewModel model)
        {
            for (int i = 0; i < model.services.Count; i++)
            {
                if (model.services[i].isChecked)
                {
                   model.services.Remove(model.services.Find(x => x.isChecked == false)); 
                }
            }
            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var id = _userManager.GetUserId(User); // Get user id:
              //  var subString = new StringBuilder(model.stime);
              var strtimeObj = _context.timeList.Find(model.stime);
              var strtime = TimeOnly.Parse(strtimeObj.strtime);
              var endTime = strtime.AddMinutes(60);
                var obj = new appointment()
                {
                    Date = DateOnly.FromDateTime(DateTime.UtcNow).ToString(),
                    barberId = model.barberId,
                    barberName = model.barber,
                    User = await _userManager.FindByIdAsync(model.customerId),
                    appointApprove = model.appointApprove,
                    shopId = model.shopId,
                    shopName = model.shop,
                    stime = strtimeObj.strtime,
                    etime = endTime.ToString("hh:mm tt")
                    
                };
                obj.service = model.services[0].name;
                obj.totalPrice = model.services[0].price;
                obj.serviceDuration = Int32.Parse(model.services[0].time);
                for (int i = 1; i < model.services.Count; i++)
                {
                    obj.service = obj.service + ", " + model.services[i].name;
                    obj.totalPrice = obj.totalPrice + model.services[i].price;
                    obj.serviceDuration = obj.serviceDuration + Int32.Parse(model.services[i].time);;
                }
                _context.appointment.Add(obj);
                await _context.SaveChangesAsync();
                return View("Index",_context.appointment.ToList());
            }
            return View(model);
        }
        public services SearchByName(string serviceName)
        {
            services service = null;
            if (!String.IsNullOrEmpty(serviceName))
            {
                //service = _context.service.FirstOrDefault(c => c.service_name == serviceName);
                service = _context.services.Where(e => e.description == serviceName).FirstOrDefault();
            }
            return service;
        }

     [HttpPost]
        public async Task<ActionResult> approve(string id)
        {
            var appointment = await _context.appointment.FindAsync(id);
            appointment.appointApprove = true;
            _context.Update(appointment);
           await _context.SaveChangesAsync();
           return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var appointment = await _context.appointment.FindAsync(id);
            _context.appointment.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
}
}
