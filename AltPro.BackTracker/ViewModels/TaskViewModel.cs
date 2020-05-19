using AltPro.BackTracker.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AltPro.BackTracker.ViewModels
{
    public class TaskViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "AssignedID")]
        public string AssignedID { get; set; }

        [Required]
        [Display(Name = "Module")]
        public EModule? ModuleName { get; set; }

        [Required]
        [Display(Name ="Priority")]
        public ETaskPriority? TaskPriority { get; set; }

        [Required]
        [MaxLength(500,ErrorMessage = "Description too long (max 500 znakis)")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name ="Attachments")]
        public List<IFormFile> Attachemnts  { get; set; }

        [Display(Name ="Comments")]
        public List<CommentModel> Comments { get; set; }
    }

}
