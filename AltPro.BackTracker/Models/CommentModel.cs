using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("TaskModel")]
        public int TaskId{ get; set; }

        public TaskModel TaskModel { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Comment too long (max 500 znakis)")]
        public string CommentBody { get; set; }

        public DateTime TimePosted { get; set; }

        public int PosterId { get; set; }
    }
}
