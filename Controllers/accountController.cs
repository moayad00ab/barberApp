using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using barber;
using barber.ViewModels;
using barber.Data;
namespace barber.Controllers;

    public class accountController : Controller
    {
        private readonly UserManager<users> _userManager;
        private readonly SignInManager<users> _signInManager;
        private readonly ApplicationDbContext _context;

        // accountController constuctor
        public accountController(UserManager<users> userManager, SignInManager<users> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        // Action to handle the post reqeust from the user to perform logout function
        [HttpPost]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        // Action for returning the register page to the user
        public IActionResult Register()
        {
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
                var user = new users { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "Admin");
                //check if the user created succsfuly 
                if (result.Succeeded)
                {
                    // await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }


                else
                {
                    ViewBag.MsgUser = "username is already taken";
                    return View("Register");
                }
            }
            //pass the model object to view,and display any validation error may happend 
            return View();
        }
        // Action for returning the login page to the user
        [HttpGet]
        public IActionResult Login()
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
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("index", "Home");
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
               EditViewModel editView = new EditViewModel();
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

    }






