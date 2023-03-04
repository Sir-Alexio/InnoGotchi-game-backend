using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUser(UserDto dto);
        public Task<string> CreateToken();
    }
}
