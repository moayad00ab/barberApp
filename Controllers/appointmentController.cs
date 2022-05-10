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
            var model = new CreateAppointmentViewModel();
            model.services = serviceList;
            model.barberId = barber.Id;
            model.shopId = barber.barbersShop;
            model.barber = barber.fName + " " + barber.lName;
            model.shop = shop.shopName;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var id = _userManager.GetUserId(User); // Get user id:
                var ser = SearchByName(model.service);
              //  var subString = new StringBuilder(model.stime);
              var strtimeObj = _context.timeList.Find(model.stime);
              var strtime = TimeOnly.Parse(strtimeObj.strtime);
              var endTime = strtime.AddMinutes(60);
                var obj = new appointment()
                {
                    Date = DateOnly.FromDateTime(DateTime.UtcNow).ToString(),
                    barberId = model.barber,
                    service = ser,
                    User = await _userManager.FindByIdAsync(id),
                    appointApprove = model.appointApprove,
                    shopId = model.shop,
                    stime = strtimeObj.strtime,
                    etime = endTime.ToString("hh:mm tt")
                    
                };
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

    }
}
