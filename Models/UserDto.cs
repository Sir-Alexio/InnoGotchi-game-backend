using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
    }
}
