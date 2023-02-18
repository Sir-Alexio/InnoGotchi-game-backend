using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories;
using InnoGotchi_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        
        public RegController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register(UserDto userDto)
        {
            User user = MakeUser(userDto);

            _repository.User.Create(user);

            _repository.Save();

            return Ok();
        }

        private User MakeUser(UserDto dto)
        {
            User user = new User();

            CreatePasswortHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Avatar = dto.Avatar;
            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;

        }

        private void CreatePasswortHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        


    }
}
