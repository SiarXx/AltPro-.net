using AltPro.BackTracker.Controllers;
using AltPro.BackTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.ViewModels
{
    public class TaskViewModel
    {
        [Display(Name = "Module")]
        public string ModuleName { get; set; }

        [Required]
        [Display(Name ="Priority")]
        public ETaskPriority? TaskPriority { get; set; }

        [Required]
        [MaxLength(500,ErrorMessage = "Description too long (max 500 znakis)")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

}
