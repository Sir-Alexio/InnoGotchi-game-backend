using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public ICollection<User>? MyColaborators { get; set; }
        public ICollection<User>? IAmColaborator { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public virtual Farm? MyFarm { get; set; }
    }
}
