using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? Avatar { get; set; } = string.Empty;
        public string? Password { get; set; }

    }
}
