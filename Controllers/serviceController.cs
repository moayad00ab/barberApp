using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using barber.Models;
using barber.Data;
using System.Runtime.InteropServices;

namespace barber.Controllers;

    public class serviceController : Controller
    {
        private readonly UserManager<users> _userManager;
        private readonly ApplicationDbContext _context;

        // serviceController constuctor
        public serviceController(UserManager<users> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
 public async Task<IActionResult> createAsync()
        {

             System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var Id = _userManager.GetUserId(User); // Get user id:
        
        var shop = await _userManager.FindByIdAsync(Id);
            services model = new services{userId = shop.Id};
            return View(model);
        }
        // Action to handle the post reqeust from the user to perform sign up function
        [HttpPost]
        public async Task<IActionResult> create(services model)
        {
            services service;
            //check if incoming model object is valid
            if (ModelState.IsValid)
            {
                service = new services {name = model.name, description = model.description, price = model.price, userId = model.userId, time = model.time};
                var result = _context.services.Add(service);
               await _context.SaveChangesAsync();  
            }
            return RedirectToAction("Index","files", _context.services.Where(a => a.userId == model.userId).ToList());
        }
         [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var service = await _context.services.FindAsync(id);
           
            _context.services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "files");
        }
            [HttpGet]
 public async Task<IActionResult> indexAsync([Optional] string Id)
        {

            if (Id == null)
            {
                
            
             System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            Id = _userManager.GetUserId(User); // Get user id:
            }
        var shop = await _userManager.FindByIdAsync(Id);
            services model = new services{userId = shop.Id};

            return View( _context.services.Where(a => a.userId == model.userId).ToList());
        }
    }