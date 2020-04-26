using BackTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.ViewModels
{
    public class ProfileDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public string PageTitle { get; set; }

    }
}
