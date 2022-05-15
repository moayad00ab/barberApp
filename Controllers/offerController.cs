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
                 System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var id = _userManager.GetUserId(User); // Get user id:
               // var offer = await _context
                if(model != null)
                {
                 // await _context.offers.Remove(model);  
                }
               

                offer = new offers {percentage = model.percentage, User = await _userManager.FindByIdAsync(id)};
                var result = _context.offers.Add(offer);
               await _context.SaveChangesAsync();  
               var servicePrice = _context.services.Where(a => a.userId == id).ToList();
                if (model.percentage > 0)
                {
                    
                
               foreach (var service in servicePrice)
               {
                   var temp = (1.0-((float)model.percentage/100.0));
                   service.price = service.price * (float)temp;
                        _context.services.Update(service);
                                       await _context.SaveChangesAsync();  


               }
                }
            }
            return RedirectToAction("Index","files", _context.offers.ToList());
        }
         [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var offer = await _context.offers.FindAsync(id);
           var servicePrice = _context.services.Where(a => a.userId == id).ToList();

                foreach (var service in servicePrice)
               {
                   var temp = ((float)offer.percentage/100.0);
                   service.price = service.price + (float)temp;
                     _context.services.Update(service);
                      await _context.SaveChangesAsync();  


               }
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