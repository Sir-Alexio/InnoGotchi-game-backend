using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IUserService
    {
        public StatusCode UpdateUser(UserDto dto);
        public StatusCode Registrate(UserDto userDto);
        public StatusCode ChangePassword(ChangePasswordModel changePassword, string email);
        public StatusCode GetUser(string email, out User? user);
    }
}
