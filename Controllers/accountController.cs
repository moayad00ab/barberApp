using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using barber.ViewModels;
using barber.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;
using System.Security.Claims;
using barber.Models;
using Microsoft.EntityFrameworkCore;

namespace barber.Controllers;

public class accountController : Controller
{
    private readonly UserManager<users> _userManager;
    private readonly SignInManager<users> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment hostingEnvironment;
    // accountController constuctor
    public accountController(UserManager<users> userManager, SignInManager<users> signInManager, ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _configuration = configuration;
        this.hostingEnvironment = hostingEnvironment;
    }

    // Action to handle the post reqeust from the user to perform logout function
    [HttpPost]
    public async Task<IActionResult> logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    // Action for returning the register page to the user
    public IActionResult Register()
    {
        ViewBag.Users = new SelectList(_userManager.Users.Where(a => a.shopName != null), nameof(users.Id), nameof(users.shopName));
        return View();
    }
    // Action to handle the post reqeust from the user to perform sign up function
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        //check if incoming model object is valid
        if (ModelState.IsValid)
        {
            //if valid, code will create new user 
            if (model.userType == "Customer")
            {
            var user = new users { fName = model.fName, lName = model.lName, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber};
            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Customer");
            }
            if (model.userType == "Barber")
            {
               var user = new users { fName = model.fName, lName = model.lName, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, barbersShop = model.barbersShop};
            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Barber");
            var shop = await _userManager.FindByIdAsync(user.barbersShop);
            var startTime = Convert.ToDateTime(shop.sWorkTime);
            var endTime = Convert.ToDateTime(shop.eWorkTime);
            var min = 60;
            var timeInterval = endTime.Subtract(startTime);
            var totalMin = Convert.ToInt32(timeInterval.TotalMinutes);
            var noOfTotalSlots = totalMin/min;
            if (shop.eWorkTime != null && shop.sWorkTime != null)
            {
            
            
            for (int j = 0; j < noOfTotalSlots; j++)
            {
                timeList obj = new timeList();
                startTime = startTime.AddMinutes(min);
                obj.strtime = startTime.ToString("hh:mm tt");
                obj.barber = user;
                _context.timeList.Add(obj);
                await _context.SaveChangesAsync();
            }  
            }
            }
            if (model.userType == "BarberShop")
            {
                if(model.shop != null){
            var user = new users { fName = model.fName, lName = model.lName, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, shopName = model.shop};
            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "BarberShop");
                }else{
                    ViewBag.shop_name = "Please enter your shop name";
                    return View(model);
                }
            }

            // await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login");



            
        }
        return View();
    }
    public users SearchByName(string shopName)
    {
        users shop = null;
        if (!String.IsNullOrEmpty(shopName))
        {
            shop = _userManager.Users.Where(e => e.shopName == shopName).FirstOrDefault();
        }
        return shop;
    }
    // Action for returning the login page to the user
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    public IActionResult settings()
    {
        return View();
    }
    // Action to handle the post reqeust from the user to perform login function
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            //if valid, code will sign in the user
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            //check if the user created succsfuly 
            if (result.Succeeded)
            {
                return RedirectToAction("index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        }

        return View(model);
    }
    // Action to handle the post reqeust from the user to perform edit function
    [HttpPost]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            model.Id = _userManager.GetUserId(User); // Get user id:
        }
        
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.barbersShop = model.barbersShop;
            user.sWorkTime = model.sWorkTime;
            user.eWorkTime = model.eWorkTime;
            user.isAvilable = model.isAvilable;

            var startTime = Convert.ToDateTime(user.sWorkTime);
            var endTime = Convert.ToDateTime(user.eWorkTime);
            var min = 60;
            var timeInterval = endTime.Subtract(startTime);
            var totalMin = Convert.ToInt32(timeInterval.TotalMinutes);
            var noOfTotalSlots = totalMin/min;

            if (model.eWorkTime != null && model.sWorkTime != null)
            {
                _context.timeList.RemoveRange(_context.timeList.Where(x => x.barber.barbersShop == user.Id));
            }
            List<users> barbers = _userManager.Users.Where(a => a.barbersShop == user.Id).ToList();
            for (int i = 0; i < barbers.Count; i++)
            {
            
            for (int j = 0; j < noOfTotalSlots; j++)
            {
                timeList obj = new timeList();
                startTime = startTime.AddMinutes(min);
                obj.strtime = startTime.ToString("hh:mm tt");
                obj.barber = barbers[i];
                _context.timeList.Add(obj);
                await _context.SaveChangesAsync();
            }  
            }
            // Update the user 
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "files");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        return View(model);
    }
    // Action for returning the edit page to the user
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        // Find the user by user ID
        EditViewModel editView = new EditViewModel();

        if (string.IsNullOrWhiteSpace(id))
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            id = _userManager.GetUserId(User); // Get user id:
        }
        // Find the user by user ID
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }
        else
        {
            editView.Id = user.Id;

            editView.Email = user.Email;
            editView.PhoneNumber = user.PhoneNumber;
            editView.UserName = user.UserName;
            editView.sWorkTime = user.sWorkTime;
            editView.eWorkTime = user.eWorkTime;
            editView.shopName = user.shopName;
            editView.isAvilable = user.isAvilable;
            editView.barbersShop = user.barbersShop;
         //   editView.isShopAvailable = user.isShopAvailable;
            return View(editView);
        }

    }
    // Action to handle the post reqeust from the user to perform delete function
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }
        else
        {

            
            if (user.barbersShop == null)
            {
                _context.slot.RemoveRange(_context.slot.Where(x => x.User.Id == user.Id));

                using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
                {
                    sqlCon.Open();

                    string query = "DELETE FROM AspNetUsers WHERE barbersShop = @Id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Id", id);
                    sqlCmd.ExecuteNonQuery();
                }
            }
            if (user.shopName == null)
            {
                _context.slot.RemoveRange(_context.slot.Where(x => x.User.Id == user.Id));

                 using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
                {
                    sqlCon.Open();

                    string query = "DELETE FROM AspNetUsers WHERE barbersShop = @Id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Id", id);
                    sqlCmd.ExecuteNonQuery();
                }

            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("listUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }


            try
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Error: " + ModelState.Values);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("ListUsers", "Account");
        }
    }
    [HttpGet]
    public IActionResult ListUsers()
    {
        var users = _userManager.Users;
        return View(users);
    }
    [HttpGet]
    public async Task<IActionResult> Shops()
    {

        return View(await _userManager.GetUsersInRoleAsync("BarberShop"));
    }
    [HttpGet]
    public async Task<IActionResult> barbers([Optional] string id)
    {
        if (id != null)
            return View(_userManager.Users.Where(a => a.barbersShop == id));
        else
            return View(await _userManager.GetUsersInRoleAsync("Barber"));
    }
    // Action for returning the customerProfile page to the user
    public async Task<IActionResult> Details([Optional] string id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        users user = await _userManager.FindByIdAsync(id);
        feedbackViewModel vm =
   new feedbackViewModel();
        if (user == null){
            return NotFound();
        }
        vm.UserId = id;
        vm.User = user.shopName;
        var comments = _context.feedback.Where(d => d.UserId.Equals(id)).ToList();
        vm.comments = comments;
        var ratings = _context.feedback.Where(d => d.UserId.Equals(id)).ToList();
   if (ratings.Count() > 0){
            var ratingSum = ratings.Sum(d => d.rating);
   ViewBag.RatingSum = ratingSum;
        var ratingCount = ratings.Count();
        ViewBag.RatingCount = ratingCount;
   }else{
            ViewBag.RatingSum = 0;
   ViewBag.RatingCount = 0;
   }
   return View(vm);   
}
[HttpGet]
public async Task<IActionResult> insights([Optional] string search){

     System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var id = _userManager.GetUserId(User); // Get user id:
            var shop =await _userManager.FindByIdAsync(id);
            var query = _context.appointment.Where(a => a.shopId == shop.Id);

                List<users> Barbers = _userManager.Users.Where(a => a.barbersShop == shop.Id).ToList(); 
                ViewBag.AppoinForBarbers = new int [Barbers.Count];    
                ViewBag.barberName = new string [Barbers.Count];
                ViewBag.TotalIbarbers = new float [Barbers.Count];

                for(int i = 0; i < Barbers.Count; i++){
                   ViewBag.AppoinForBarbers [i] = _context.appointment.Where(a => a.barberId == Barbers[i].Id).Count();
                   ViewBag.barberName[i] = Barbers[i].fName + " " + Barbers[i].lName;

                }

    List<appointment> numOfAppointments = _context.appointment.Where(a => a.shopId == shop.Id).ToList();    
           ViewData["Search"] = search;
            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Date.Contains(search) && x.shopId == shop.Id);
            }
            
    ViewBag.DynamicPricing = DynamicPricing(shop);
 List<appointment> numOfAppointments30 = _context.appointment.Where(a => a.shopId == shop.Id).ToList();
 List<appointment> TotalIncome = _context.appointment.Where(a => a.shopId == shop.Id).ToList();
 
    ViewBag.TIncome =(float) 0;
    ViewBag.last30Income = (float) 0;
    int count = 0;
    var currentDate = DateOnly.FromDateTime(DateTime.Now);
    ViewBag.last30 = new List<appointment>();


    for (int i = 0; i < 30; i++)
    {
      ViewBag.last30  = _context.appointment.Where(a => a.Date == currentDate.ToString() && a.shopId == shop.Id).ToList(); // Last 30 appointments
       // if (ViewBag.last30 != null)
      //  {
       //            ViewBag.last30Income =ViewBag.last30Income + ViewBag.last30[i].totalPrice; // calculating last 30 days income

        //}
        
     ViewBag.last30Appoint  = ViewBag.last30Appoint + _context.appointment.Where(a => a.Date == (currentDate.AddDays(0)).ToString() && a.shopId == shop.Id).ToList().Count(); // Last 30 appointments

       currentDate = currentDate.AddDays(-1);
    }


foreach (var price in numOfAppointments)
{
    
    ViewBag.TIncome = (float) ViewBag.TIncome + price.totalPrice; // calculating the total income
       // ViewBag.TotalIbarbers[count] = ViewBag.TotalIbarbers[count] + _context.appointment.Where(a => a.barberId == Barbers[count].Id); 
//count++;
}


foreach (var barber in Barbers)
{

}
 


    ViewBag.appointNum = numOfAppointments.Count.ToString();
    return View(await query.AsNoTracking().ToListAsync());
}
public string DynamicPricing(users shop)
        {
            

                var today = DateOnly.FromDateTime(DateTime.Today);
                var oneWeek = today.AddDays(-7);
                var twoWeek = today.AddDays(-14);
                var NumOfAppoint7 = _context.appointment.Where(a => a.Date == oneWeek.ToString() && a.shopId == shop.Id).Count();
               var NumOfAppoint14 = _context.appointment.Where(a => a.Date == twoWeek.ToString() && a.shopId ==  shop.Id).Count();
               var percentage = 0;

                if (NumOfAppoint14 != 0)
                {
                percentage = (NumOfAppoint7/NumOfAppoint14)*100;
                }
               
               if(NumOfAppoint7 == 0)
               {
                percentage = 100;
               }

               if (NumOfAppoint7<NumOfAppoint14)
               {
                   return "We suggest that you to add an offer for today, because your appointments has been decreased by: %"+ percentage ;
               }

            return null;
        }

             [HttpPost]
        public async Task<ActionResult> isShopAvailable()
        {

           System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var id = _userManager.GetUserId(User); // Get user id:
            var shop =await _userManager.FindByIdAsync(id);

            if (!shop.isShopAvailable)
            {
              shop.isShopAvailable = true;

            }else
            {
              shop.isShopAvailable = false;

            }
              await _userManager.UpdateAsync(shop);
           return RedirectToAction("Index", "files");
        }

             [HttpPost]
        public async Task<ActionResult> isAvailable(string id)
        {

            var appointment = await _context.appointment.FindAsync(id);
            var barber = await _userManager.FindByIdAsync(appointment.barberId);
            barber.numOfTotalAppoint = barber.numOfTotalAppoint +1;
              await _userManager.UpdateAsync(barber);
            appointment.appointApprove = true;
            _context.Update(appointment);
           await _context.SaveChangesAsync();
           return RedirectToAction("Index", "files");
        }
        [HttpPost]
public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        // ChangePasswordAsync changes the user password
        var result = await _userManager.ChangePasswordAsync(user,
            model.CurrentPassword, model.NewPassword);

        // The new password did not meet the complexity rules or
        // the current password is incorrect. Add these errors to
        // the ModelState and rerender ChangePassword view
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        // Upon successfully changing the password refresh sign-in cookie
        await _signInManager.RefreshSignInAsync(user);
        return View("ChangePasswordConfirmation");
    }

    return View(model);
}
[HttpGet]
public IActionResult ChangePassword()
{
    
    return View();
}
}






