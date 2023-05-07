using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IAuthenticationService
    {
        public bool ValidateUser(string password,string email);
        public Task<string> CreateToken();
    }
}
