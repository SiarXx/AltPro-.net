using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AltPro.BackTracker.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        public IActionResult NewTask()
        {
            return View();
        }
    }
}