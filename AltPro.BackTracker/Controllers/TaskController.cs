using System;
using System.Collections.Generic;
using System.Linq;
using AltPro.BackTracker.Models;
using AltPro.BackTracker.Models.Enums;
using AltPro.BackTracker.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AltPro.BackTracker.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository taskRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TaskController(ITaskRepository taskRepository,IWebHostEnvironment webHostEnvironment)
        {
            this.taskRepository = taskRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        TaskViewModel newTask = new TaskViewModel();

        [HttpGet]
        public IActionResult AddTask()
        {
            return View(newTask);
        }

        [HttpPost]
        public IActionResult AddTask(TaskViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                TaskModel task = new TaskModel()
                {
                    TaskTitle = model.Title,
                    AssignedID = model.AssignedID,
                    ModuleName = Enum.GetName(typeof(EModule),model.ModuleName),
                    TaskPriority = model.TaskPriority,
                    TaskState = ETaskState.Reported,
                    Description = model.Description,
                    ReporterID = User.Identity.GetUserId()
            };
                taskRepository.Add(task);
                return RedirectToAction("AddTask");
            }
            return View();
        }
    }

}