using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AltPro.BackTracker.Models;
using AltPro.BackTracker.Models.Enums;
using AltPro.BackTracker.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AltPro.BackTracker.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository TaskRepository;
        private readonly IWebHostEnvironment HostEnvironment;

        public TaskController(ITaskRepository taskRepository,IWebHostEnvironment webHostEnvironment)
        {
            this.TaskRepository = taskRepository;
            this.HostEnvironment = webHostEnvironment;
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
                TaskRepository.Add(task);

                string uniqueFileName = null;

                if (model.Attachemnts != null && model.Attachemnts.Count > 0)
                {
                    foreach (IFormFile file in model.Attachemnts)
                    {
                        string uplodasFolder = Path.Combine(HostEnvironment.WebRootPath, "attachments");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uplodasFolder, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                        Attachment attachment = new Attachment()
                        {
                            Path = uniqueFileName,
                            TaskId = task.TaskModelId
                        };
                        TaskRepository.Add(attachment);
                    }
                }

                    return RedirectToAction("AddTask");
            }
            return View();
        }
    }

}