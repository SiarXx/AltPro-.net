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

namespace AltPro.BackTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
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
    }
}
