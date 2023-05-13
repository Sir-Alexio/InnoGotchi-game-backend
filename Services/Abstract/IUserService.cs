using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IUserService
    {
        public Task<bool> UpdateUser(UserDto dto);
        public Task<bool> Registrate(UserDto userDto);
        public Task<bool> ChangePassword(ChangePasswordModel changePassword, string email);
        public Task<User> GetUser(string email);
        public Task<List<User>> GetAll();
    }
}
