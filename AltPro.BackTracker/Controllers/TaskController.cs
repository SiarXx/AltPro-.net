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
                    ModuleName = Enum.GetName(typeof(EModule),model.ModuleName),
                    TaskPriority = model.TaskPriority,
                    TaskState = ETaskState.Reported,
                    Description = model.Description,
                    ReporterID = User.Identity.GetUserId()
                };
                TaskRepository.Add(task);

                foreach (var file in model.Attachemnts)
                {
                    Attachment attachment = new Attachment()
                    {
                        Path = ProcessUploadFile(model),
                        TaskId = task.TaskModelId
                    };
                    TaskRepository.Add(attachment);
                }
                
                return RedirectToAction("AddTask");
            }
            return View();
        }

        private string ProcessUploadFile(TaskViewModel model)
        {
            string uniqueFileName = null;
            if (model.Attachemnts != null && model.Attachemnts.Count > 0)
            {
                foreach(IFormFile attachment in model.Attachemnts) { 
                    string uplodasFolder = Path.Combine(HostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + attachment.FileName;
                    string filePath = Path.Combine(uplodasFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        attachment.CopyTo(fileStream);
                    }
                }

                
            }

            return uniqueFileName;
        }
    }

}