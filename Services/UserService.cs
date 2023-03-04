using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using System.Security.Cryptography;

namespace InnoGotchi_backend.Services
{
    public class UserService:IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IAuthenticationService _authentication;
        public UserService(IRepositoryManager repository, IAuthenticationService authentication)
        {
            _repository = repository;
            _authentication = authentication;
        }

        public async Task<bool> UpdateUser(UserDto dto)
        {
            User? user = _repository.User.GetUserByEmail(dto.Email);

            user.UserName = dto.UserName;

            user.FirstName = dto.FirstName;

            user.LastName = dto.LastName;

            user.Avatar = dto.Avatar;

            _repository.User.Update(user);

            _repository.Save();

            return true;
        }

        public async Task ChangePassword(ChangePasswordModel changePassword, string email)
        {
            User? currentUser = _repository.User.GetUserByEmail(email);

            UserDto dto = new UserDto();

            dto.Password = changePassword.CurrentPassword;

            dto.Email = email;

            if (!_authentication.ValidateUser(dto).Result)
            {
                throw new CustomExeption("Password is incorrect") { StatusCode = ((int)ErrorStatus.WrongPassword) };
            }

            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            _repository.User.Update(currentUser);

            _repository.Save();
        }
        public async Task<bool> Registrate(UserDto userDto)
        {
            User user = MakeUser(userDto);

            _repository.User.Create(user);

            _repository.Save();

            return true;
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
