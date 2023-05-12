using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        public List<User> GetAll()
        {
            //Get all users from the database
            List<User> users = _repository.User.GetAll(trackChanges:true).ToList();

            if (users == null)
            {
                throw new CustomExeption(message: "No users found") { StatusCode = StatusCode.DoesNotExist };
            }

            return users;
        }

        public bool UpdateUser(UserDto dto)
        {
            //Get user from database
            User? user = _repository.User.GetUserByEmail(dto.Email);

            if (user == null)
            {
                return false;
            }

            //Map UserDto to User entity
            user = _mapper.Map<User>(dto);

            //Update entity
            _repository.User.Update(user);

            try
            {
                //Try save changes to the database
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public bool ChangePassword(ChangePasswordModel changePassword, string email)
        {
            //Get current user by email
            User? currentUser = _repository.User.GetUserByEmail(email);

            //Validate old password
            if (!_authentication.ValidateUser(changePassword.CurrentPassword, email))
            {
                return false;
            }

            //Create hash and salt of new password
            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            //Set hash and salt to entity we got from the database
            //It's important before updating we must get entity from the database in current method
            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            _repository.User.Update(currentUser);

            //Try update changes
            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message:"Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public bool Registrate(UserDto userDto)
        {
            //Create user in private method
            User user = MakeUser(userDto);

            //Check if email for user is alredy exist
            if (_repository.User.GetUserByEmail(user.Email) != null)
            {
                return false;
            }

            _repository.User.Create(user);

            //Check for updating entity
            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public User GetUser(string email)
        {
            User? user = _repository.User.GetUserByEmail(email);

            if (user == null)
            {
                throw new CustomExeption(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }

            return user;
        }

        //Private method for creating new user
        private User MakeUser(UserDto dto)
        {
            User user = new User();

            //Create password hash
            CreatePasswortHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user = _mapper.Map<User>(dto);

            user.Password = passwordHash;

            user.PasswordSalt = passwordSalt;

            return user;

        }

        //Private method for creating password hash and salt
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
