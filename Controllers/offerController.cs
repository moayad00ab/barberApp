using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using barber.Models;
using barber.Data;
namespace barber.Controllers;

    public class offerController : Controller
    {
        private readonly UserManager<users> _userManager;
        private readonly ApplicationDbContext _context;

        // offerController constuctor
        public offerController(UserManager<users> userManager, ApplicationDbContext context)
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
        public async Task<IActionResult> create(offers model)
        {
            offers offer;
            //check if incoming model object is valid
            if (ModelState.IsValid)
            {
                offer = new offers {description = model.description, User = model.User};
                var result = _context.offers.Add(offer);
               await _context.SaveChangesAsync();  
            }
            return View("Index", _context.offers.ToList());
        }
         [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var offer = await _context.offers.FindAsync(id);
           
            _context.offers.Remove(offer);
            await _context.SaveChangesAsync();
            return View("Index", _context.offers.ToList());
        }
            [HttpGet]
 public IActionResult index()
        {
            return View(_context.offers.ToList());
        }
    }