using AutoMapper;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Repositories;
using InnoGotchi_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly IMapper _mapper;
        private readonly IAuthenticationManager _authenticationManager;

        public RegController(IRepositoryManager repository, IMapper mapper, IAuthenticationManager authenticationManager)
        {
            _repository = repository;
            _mapper = mapper;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register(UserDto userDto)
        {
            User user = MakeUser(userDto);

            _repository.User.Create(user);

            _repository.Save();

            return Ok();
        }

        [HttpPatch]
        [Authorize]

        public async Task<ActionResult<string>> ChangePassword(ChangePasswordModel changePassword)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            User? currentUser = _repository.User.GetUserByEmail(email);

            UserDto dto = new UserDto();

            dto.Password = changePassword.CurrentPassword;
            dto.Email = email;

            if (!_authenticationManager.ValidateUser(dto).Result)
            {
                return BadRequest("Wrong password");
            }

            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            _repository.User.Update(currentUser);
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
