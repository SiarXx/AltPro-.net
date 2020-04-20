using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.ViewModels
{
    public class ProfileEditViewModel : RegisterViewModel
    {
        public string Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
