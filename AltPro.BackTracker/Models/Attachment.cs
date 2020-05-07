using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public string Path { get; set; }
    }
}
