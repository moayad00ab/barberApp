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
        var model = new RegisterViewModel(){
            barbersShop = "select"
        };
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
            var user = new users { fName = model.fName, lName = model.lName, UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Customer");
            /**   if (model.shop == null && model.barbersShop != null)
               {
                   user.barbersShop = model.barbersShop;
                   await _userManager.CreateAsync(user, model.Password);
                   await _userManager.AddToRoleAsync(user, "Barber");
               }
               if (model.barbersShop == null && model.shop != null)
               {
                   user.shopName = model.shop;
                   await _userManager.CreateAsync(user, model.Password);
                   await _userManager.AddToRoleAsync(user, "BarberShop");
               }
               if (model.shop == null && model.barbersShop == null)
               {
                   await _userManager.CreateAsync(user, model.Password);
                   await _userManager.AddToRoleAsync(user, "Customer");
               }**/
            //check if the user created succsfuly 


            // await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login");



            //   else
            // {
            //   ViewBag.MsgUser = "username is already taken";
            // return View("Register");
            //}
        }
        //pass the model object to view,and display any validation error may happend 
        return View();
    }
    public users SearchByName(string shopName)
    {
        users shop = null;
        if (!String.IsNullOrEmpty(shopName))
        {
            //shop = __context.shop.FirstOrDefault(c => c.shop_name == shopName);
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
        else
        {
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.barbersShop = model.barbersShop;

            // Update the user 
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return View("ListUsers", _userManager.Users);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }


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
}






