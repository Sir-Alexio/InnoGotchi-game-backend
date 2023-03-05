using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IUserService
    {
        public Task<bool> UpdateUser(UserDto dto);
        public Task<StatusCode> Registrate(UserDto userDto);
        public Task<StatusCode> ChangePassword(ChangePasswordModel changePassword, string email);
    }
}
