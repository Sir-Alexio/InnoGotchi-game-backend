using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IUserService
    {
        public bool UpdateUser(UserDto dto);
        public bool Registrate(UserDto userDto);
        public bool ChangePassword(ChangePasswordModel changePassword, string email);
        public User GetUser(string email);
        public List<User> GetAll();
    }
}
