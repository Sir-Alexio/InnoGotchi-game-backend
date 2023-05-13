using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.Entity
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
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public virtual Farm? MyFarm { get; set; }
    }
}
