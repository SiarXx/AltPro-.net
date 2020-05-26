using AltPro.BackTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.ViewModels
{
    public class TaskEditViewModel : TaskViewModel
    {
        public int Id { get; set; }

        [MaxLength(500, ErrorMessage = "Description too long (max 500 znakis)")]
        public string NewCommentBody { get; set; }
    }
}
