using AltPro.BackTracker.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.ViewModels
{
    public class TaskViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int TaskID { get; set; }
        [Display(Name = "Module")]
        public string ModuleName { get; set; }

        [Required]
        [Display(Name = "ReporterID")]
        public string ReporterID { get; set; }

        [Required]
        [Display(Name ="Priority")]
        public ETaskPriority TaskPriority { get; set; }

        [Display(Name = "Status")]
        public string TaskStatus { get; set; }
    }
    public enum ETaskPriority
    {
        low = 1,
        normal = 2,
        high = 3
    }
}
