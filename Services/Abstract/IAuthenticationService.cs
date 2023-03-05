using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUser(User user);
        public Task<string> CreateToken();
    }
}
