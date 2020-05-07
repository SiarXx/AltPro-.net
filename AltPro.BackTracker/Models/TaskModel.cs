using AltPro.BackTracker.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class TaskModel
    {
        
        
        public int TaskModelId { get; set; }

        [Required]
        public string TaskTitle { get; set; }

        [Required]
        public string ModuleName { get; set; }

        [Required]
        public string ReporterID { get; set; }

        public string AssignedID { get; set; }

        [Required]
        public ETaskPriority? TaskPriority { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ETaskState? TaskState { get; set; }

        public List<string> AttachmentsPaths { get; set; }

    }
}
