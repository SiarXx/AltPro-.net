using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltPro.BackTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AltPro.BackTracker.Controllers
{
    public class TaskController : Controller
    {
        TaskViewModel newTask = new TaskViewModel();
        [HttpGet]
        public IActionResult NewTask()
        {
            newTask.ReporterID = "SampleReporter2137";
            return View(newTask);
        }
    }

}