using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class TaskModel
    {
        public TaskModel() { }
        public int TaskID { get; set; }
        [Required]
        public string ModuleName { get; set; }
        [Required]
        public string ReporterID { get; set; }
        [Required]
        public ETaskPriority? TaskPriority { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TaskSta { get; set; }
     
    }
}
