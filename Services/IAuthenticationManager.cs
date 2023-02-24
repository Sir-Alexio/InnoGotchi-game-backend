using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserDto dto);
        public Task<string> CreateToken();
    }
}
