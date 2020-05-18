using AltPro.BackTracker.Models;
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
    }

}
