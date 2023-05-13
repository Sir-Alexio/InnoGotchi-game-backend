using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.Entity
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
