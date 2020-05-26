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
using AltPro.BackTracker.Models.Enums;

namespace AltPro.BackTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller

    {
        private ITaskRepository reportRepository;
        private readonly IWebHostEnvironment HostEnvironment;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManage;
        private readonly AppDBContext Context;

        public HomeController(IWebHostEnvironment webHostEnvironment,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            AppDBContext context,
            ITaskRepository reportRepository)
        {
            this.userManage = userManager;
            this.HostEnvironment = webHostEnvironment;
            this.reportRepository = reportRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Identity.GetUserId();
            var user = await userManage.Users.FirstOrDefaultAsync(e => e.Id == userId);

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
        public async Task<ViewResult> Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = await userManage.Users.FirstOrDefaultAsync(e => e.Id == userId);

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
                var user = await userManage.Users.FirstOrDefaultAsync(e => e.Id == userId);
                
                if(model.Email != null)
                {
                    user.Email = model.Email;
                }

                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(HostEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    user.PhotoPath = ProcessUploadFile(model);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        

        public ViewResult ReportList(string searchString)
        {
            var tasks = reportRepository.GetAllTasks();

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = reportRepository.GetAllTasks().Where(s => s.TaskTitle.ToUpper().Contains(searchString.ToUpper()));
            }
            var model = tasks.ToList();
            return View(model);
        }

        public IActionResult UserReportList()
        {
            var tasks = new List<TaskModel>();
            var querry = reportRepository.GetAllTasks().Where(s => s.ReporterID.Equals(User.Identity.GetUserId())|| s.AssignedID.Equals(User.Identity.GetUserId()));
            tasks = querry.ToList();
            return View(tasks);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult TaskView(int id)
        {
            TaskModel taskModel = reportRepository.GetTask(id);
            Enum.TryParse(taskModel.ModuleName, out EModule module);

            TaskEditViewModel editTaskModel = new TaskEditViewModel
            {
                Id = id,
                Title = taskModel.TaskTitle,
                AssignedID = taskModel.AssignedID,
                ModuleName = module,
                TaskPriority = taskModel.TaskPriority,
                Description = taskModel.Description,
                Comments = LoadComments(id)

            };
            return View(editTaskModel);
        }

        [HttpPost]
        public IActionResult TaskView(TaskEditViewModel model)
        {
            if(model.NewCommentBody!= null)
            {
                CommentModel comment = new CommentModel()
                {
                    CommentBody = model.NewCommentBody,
                    PosterName = User.Identity.Name,
                    TimePosted = DateTime.Now,
                    TaskId = model.Id
                };
                reportRepository.AddComment(comment);
            }
                
            return TaskView(model.Id);
        }

        [HttpGet]
        public ViewResult EditTask(int id)
        {
            TaskModel taskModel = reportRepository.GetTask(id);
            Enum.TryParse(taskModel.ModuleName, out EModule module);
            TaskEditViewModel editTaskModel = new TaskEditViewModel
            {
                Title = taskModel.TaskTitle,
                AssignedID = taskModel.AssignedID,
                ModuleName = module,
                TaskPriority = taskModel.TaskPriority,
                Description = taskModel.Description
            };
            return View(editTaskModel);
        }

        [HttpPost]
        public IActionResult EditTask(TaskEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                TaskModel task = reportRepository.GetTask(model.Id);
                task.TaskTitle = model.Title;
                task.AssignedID = model.AssignedID;
                task.ModuleName = model.ModuleName.ToString();
                task.TaskPriority = model.TaskPriority;
                task.TaskState = ETaskState.Reported;
                task.Description = model.Description;

                reportRepository.Edit(task);
                return RedirectToAction("ReportList");
            }
            return View();
        }

        public IActionResult DeleteTask(int id)
        {
            TaskModel taskModel = reportRepository.GetTask(id);
            Enum.TryParse(taskModel.ModuleName, out EModule module);
            TaskEditViewModel editTaskModel = new TaskEditViewModel
            {
                ModuleName = module,
                TaskPriority = taskModel.TaskPriority,
                Description = taskModel.Description
            };

            reportRepository.Delete(id);
            return View(editTaskModel);
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string ProcessUploadFile(ProfileEditViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uplodasFolder = Path.Combine(HostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uplodasFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public List<CommentModel> LoadComments(int id)
        {
            var comments = reportRepository.GetAllComments(id).ToList();
            return comments;
        }
        

    }
}
