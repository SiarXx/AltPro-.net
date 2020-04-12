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
        public int TaskID { get; set; }
        public string ModuleName { get; set; }

        [Required]
        public string ReporterID { get; set; }

        public string TaskPriority { get; set; }
        public string TaskStatus { get; set; }
    }
}
