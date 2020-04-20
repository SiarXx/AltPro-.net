using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AltPro.BackTracker.Models;
using Microsoft.AspNetCore.Authorization;
using AltPro.BackTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using BackTracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AltPro.BackTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly AppDBContext context;

        public HomeController(ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            AppDBContext context)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Identity.GetUserId();
            var user = await userManager.Users.FirstOrDefaultAsync(e => e.Id == userId);

            if(user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", userId);
            }

            ProfileDetailsViewModel profileDetailsViewModel = new ProfileDetailsViewModel()
            {
                User = user,
                PageTitle = "User Profile"
            };

            return View(profileDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = await userManager.Users.FirstOrDefaultAsync(e => e.Id == userId);

            ProfileEditViewModel profileEditViewModel = new ProfileEditViewModel()
            {
                Id = userId,
                Email = user.Email,
                ExistingPhotoPath = user.PhotoPath
            };
            return View(profileEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = await userManager.Users.FirstOrDefaultAsync(e => e.Id == userId);

                user.Email = model.Email;
                
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    user.PhotoPath = ProcessUploadFile(model);
                }

                var usser = context.Users.Attach(user);
                usser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string ProcessUploadFile(RegisterViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uplodasFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uplodasFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
