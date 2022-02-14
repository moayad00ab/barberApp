using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using barber.Models;
using barber.Data;
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
 public IActionResult create()
        {
            return View();
        }
        // Action to handle the post reqeust from the user to perform sign up function
        [HttpPost]
        public async Task<IActionResult> create(services model)
        {
            services service;
            //check if incoming model object is valid
            if (ModelState.IsValid)
            {
                service = new services {description = model.description, price = model.price, User = model.User};
                var result = _context.services.Add(service);
               await _context.SaveChangesAsync();  
            }
            return View("Index", _context.services.ToList());
        }
         [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var service = await _context.services.FindAsync(id);
           
            _context.services.Remove(service);
            await _context.SaveChangesAsync();
            return View("Index", _context.services.ToList());
        }
            [HttpGet]
 public IActionResult index()
        {
            return View(_context.services.ToList());
        }
    }