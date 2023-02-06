using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    public class User
    {
        [Key]
        public int UserId{ get; set; }
        [Required]
        public string UserName{ get; set; }
        [Required]
        public string FirstName{ get; set; }
        [Required]
        public string LastName{ get; set; }
        [Required]
        public string Email{ get; set; }
        public string? Avatar { get; set; }
        public int? FarmId{ get; set; }
        public List<User>? Colaborators { get; set; }
        public List<Farm>? FarmsColaborators { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
