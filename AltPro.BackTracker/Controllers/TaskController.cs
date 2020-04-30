using System;
using System.Collections.Generic;
using System.Linq;
using AltPro.BackTracker.Models;
using AltPro.BackTracker.Models.Enums;
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
            return View(newTask);
        }

        [HttpPost]
        public IActionResult AddReport(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                TaskModel task = new TaskModel()
                {
                    ModuleName = model.ModuleName,
                    TaskPriority = model.TaskPriority,
                    TaskState = ETaskState.Reported,
                    Description = model.Description,
                    ReporterID = "Tutaj wjebac id zalogowanego"
                };
            }
            return View();
        }
    }

}