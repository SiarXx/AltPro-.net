using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class FileAttachment
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("TaskModel")]
        public int TaskId { get; set; }
        public TaskModel TaskModel {get; set;}

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }
    }
}
