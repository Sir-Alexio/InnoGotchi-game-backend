using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchi_backend.Services
{
    public class UserService:IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IAuthenticationService _authentication;
        private readonly IMapper _mapper;
        public UserService(IRepositoryManager repository, IAuthenticationService authentication, IMapper mapper)
        {
            _repository = repository;
            _authentication = authentication;
            _mapper = mapper;
        }

        public StatusCode UpdateUser(UserDto dto)
        {
            User? user = _repository.User.GetUserByEmail(dto.Email);

            user.UserName = dto.UserName;

            user.FirstName = dto.FirstName;

            user.LastName = dto.LastName;

            user.Avatar = dto.Avatar;

            _repository.User.Update(user);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }

            return StatusCode.Ok;
        }

        public StatusCode ChangePassword(ChangePasswordModel changePassword, string email)
        {
            User? currentUser = _repository.User.GetUserByEmail(email);

            if (_authentication.ValidateUser(changePassword.CurrentPassword,email) == StatusCode.WrongPassword)
            {
                return StatusCode.WrongPassword;
            }

            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            _repository.User.Update(currentUser);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }
            
            return StatusCode.Ok;
        }

        public StatusCode Registrate(UserDto userDto)
        {
            User user = MakeUser(userDto);

            if (_repository.User.GetUserByEmail(user.Email) != null)
            {
                return StatusCode.IsAlredyExist;
            }

            _repository.User.Create(user);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }

            return StatusCode.Ok;
        }

        public StatusCode GetUser(string email,out User? user)
        {
            user = _repository.User.GetUserByEmail(email);
            if (user == null)
            {
                return StatusCode.DoesNotExist;
            }
            return StatusCode.Ok;
        }
        private User MakeUser(UserDto dto)
        {
            User user = new User();

            CreatePasswortHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            //will be simplify later with mapper
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
