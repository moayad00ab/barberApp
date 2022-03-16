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

namespace barber.Controllers;
public class filesController : Controller
{
    private readonly UserManager<users> _userManager;
    private readonly SignInManager<users> _signInManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment hostingEnvironment;
    // filesController constuctor
    public filesController(UserManager<users> userManager, SignInManager<users> signInManager, ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _configuration = configuration;
        this.hostingEnvironment = hostingEnvironment;
    }
[HttpGet]
    public IActionResult Index()
    {
        System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        var id = _userManager.GetUserId(User);
        return View(_context.files.Where(a => a.User.Id == id).ToList());
    }
    // POST: files/Upload
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }
    [HttpPost]
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(UploadFileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var file = new files();
            String uploadedFolder = Path.Combine(hostingEnvironment.WebRootPath, "Uploads");
            String uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImgPath.FileName;
            String filePath = Path.Combine(uploadedFolder, uniqueFileName);
            model.ImgPath.CopyTo(new FileStream(filePath, FileMode.Create));
            file.ifProfileImg = false;
            file.ImgPath = uniqueFileName;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var id = _userManager.GetUserId(User);
            file.User = await _userManager.FindByIdAsync(id); // Get user id:
            _context.Add(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }
}