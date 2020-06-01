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
using System.Net.Mail;

namespace AltPro.BackTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller

    {
        private ITaskRepository reportRepository;
        private readonly IWebHostEnvironment HostEnvironment;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManage;

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
            var querry = reportRepository.GetAlLUserTasks(User.Identity.GetUserId());
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

            var attachmentStrings = reportRepository.GetAttachmentsStrings(id);

            TaskEditViewModel editTaskModel = new TaskEditViewModel
            {
                Id = id,
                Title = taskModel.TaskTitle,
                AssignedID = taskModel.AssignedID,
                ModuleName = module,
                TaskPriority = taskModel.TaskPriority,
                Description = taskModel.Description,
                Comments = LoadComments(id),
                AttachmentStrings = attachmentStrings
            };
            return View(editTaskModel);
        }

        [HttpPost]
        public IActionResult TaskView(TaskEditViewModel model)
        {
            if (model.NewCommentBody != null)
            {
                CommentModel comment = new CommentModel()
                {
                    CommentBody = model.NewCommentBody,
                    PosterName = User.Identity.Name,
                    TimePosted = DateTime.Now,
                    TaskId = model.Id
                };
                reportRepository.AddComment(comment);
                TaskModel task = reportRepository.GetTask(model.Id);
                SendMail(task.ReporterID, task.AssignedID, task.TaskTitle);
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
                string uniqueFileName = null;

                task.TaskTitle = model.Title;
                task.AssignedID = model.AssignedID;
                task.ModuleName = model.ModuleName.ToString();
                task.TaskPriority = model.TaskPriority;
                task.TaskState = ETaskState.Reported;
                task.Description = model.Description;

                if (model.Attachemnts != null && model.Attachemnts.Count > 0)
                {
                    foreach (IFormFile file in model.Attachemnts)
                    {
                        string uplodasFolder = Path.Combine(HostEnvironment.WebRootPath, "attachments");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uplodasFolder, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                        FileAttachment attachment = new FileAttachment()
                        {
                            Path = uniqueFileName,
                            TaskId = task.TaskModelId,
                            Name = file.FileName
                        };
                        reportRepository.Add(attachment);
                    }
                }

                reportRepository.Edit(task);
                SendMail(task.ReporterID, task.AssignedID, task.TaskTitle);
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

        public ActionResult DownloadFile(string path)
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "FolderName/";
            string fileFolder = Path.Combine(HostEnvironment.WebRootPath, "attachments");
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(fileFolder, path));
            //string fileName = "filename.extension";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
        }

        [HttpGet]
        public IActionResult AssignToMe(int id)
        {
            TaskModel taskToEdit = reportRepository.GetTask(id);
            TaskModel task = reportRepository.GetTask(id);

            if (task.TaskState != ETaskState.Resolved)
            {
                task.TaskTitle = taskToEdit.TaskTitle;
                task.AssignedID = User.Identity.GetUserId();
                task.ModuleName = taskToEdit.ModuleName.ToString();
                task.TaskPriority = taskToEdit.TaskPriority;
                task.TaskState = ETaskState.Assigned;
                task.Description = taskToEdit.Description;
                reportRepository.Edit(task);
                SendMail(task.ReporterID, User.Identity.GetUserId(), task.TaskTitle);
            }

            return RedirectToAction("ReportList");
        }

        public IActionResult ResolveTask(int id) 
        {
            TaskModel taskToEdit = reportRepository.GetTask(id);
            TaskModel task = reportRepository.GetTask(id);

            task.TaskTitle = taskToEdit.TaskTitle;
            task.AssignedID = User.Identity.GetUserId();
            task.ModuleName = taskToEdit.ModuleName.ToString();
            task.TaskPriority = taskToEdit.TaskPriority;
            task.TaskState = ETaskState.Resolved;
            task.Description = taskToEdit.Description;
            reportRepository.Edit(task);
            SendMail(task.ReporterID, User.Identity.GetUserId(), task.TaskTitle);


            return RedirectToAction("ReportList");
        }

        async private void SendMail(string ownerId, string assignedId, string taskTitle)
        {

            var owner = userManage.FindByIdAsync(ownerId).Result;

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sendnotifiactionemail@gmail.com");
                message.To.Add(owner.Email);

                if (assignedId != null)
                {
                    var assigned = userManage.FindByIdAsync(assignedId).Result;
                    message.To.Add(assigned.Email);

                }

                message.Subject = "Zmiany w " + taskTitle;
                message.Body = "W tasku " + taskTitle + " zostały wprowadozne zmiany";
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("sendnotifiactionemail@gmail.com", "H4$l00!qAz");
                client.Send(message);
                message = null;


            }

            catch (Exception ex)
            {

            }
        }

    }
}
