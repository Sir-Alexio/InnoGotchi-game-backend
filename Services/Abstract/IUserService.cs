using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IUserService
    {
        public Task<bool> UpdateUser(UserDto dto);
        public   Task<bool> Registrate(UserDto userDto);
        public Task ChangePassword(ChangePasswordModel changePassword, string email);
    }
}
