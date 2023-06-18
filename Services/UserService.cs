using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<User>> GetUsersWithNoInvited(string email)
        {
            //Get from data base all users
            List<User> allUsersWithNoInvited =  await _repository.User.GetAll(trackChanges: false).Result.ToListAsync();

            //Get from data base current user
            User? currentUser = await _repository.User.GetUserWithColaboratorsAsync(email);

            //Check current user
            if (currentUser == null)
            {
                throw new CustomExeption(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }

            //mistake here
            //Get current user collaborators
            List<User>? invitedUsers = currentUser.MyColaborators.ToList();

            //Remove invited and current users
            allUsersWithNoInvited.RemoveAll(user => invitedUsers.Contains(user));

            allUsersWithNoInvited.Remove(currentUser);

            return allUsersWithNoInvited;
        }

        public async Task<List<User>> GetAll()
        {
            //Get all users from the database
            List<User> users = await _repository.User.GetAll(trackChanges:true).Result.ToListAsync();

            if (users == null)
            {
                throw new CustomExeption(message: "No users found") { StatusCode = StatusCode.DoesNotExist };
            }

            return users;
        }

        public async Task<bool> UpdateUser(UserDto dto)
        {
            //Get user from database
            User? user = await _repository.User.GetUserByEmail(dto.Email);

            if (user == null)
            {
                return false;
            }

            // Partial map UserDto to User entity

            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Avatar = dto.Avatar;

            try
            {
                //Update entity
                await _repository.User.Update(user);

                //Try save changes to the database
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> ChangePassword(ChangePasswordModel changePassword, string email)
        {
            //Get current user by email
            User? currentUser = await _repository.User.GetUserByEmail(email);

            bool isUserValid = await _authentication.ValidateUser(changePassword.CurrentPassword, email);

            //Validate old password
            if (!isUserValid)
            {
                return false;
            }

            //Create hash and salt of new password
            CreatePasswortHash(changePassword.NewPassword, out byte[] hash, out byte[] salt);

            //Set hash and salt to entity we got from the database
            //It's important before updating we must get entity from the database in current method
            currentUser.Password = hash;

            currentUser.PasswordSalt = salt;

            await _repository.User.Update(currentUser);

            //Try update changes
            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message:"Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> Registrate(UserDto userDto)
        {
            //Create user in private method
            User user = MakeUser(userDto);

            //Check if email for user is alredy exist
            if (await _repository.User.GetUserByEmail(user.Email) != null)
            {
                return false;
            }

            await _repository.User.Create(user);

            //Check for updating entity
            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<User> GetUser(string email)
        {
            User? user = await _repository.User.GetUserByEmail(email);

            if (user == null)
            {
                throw new CustomExeption(message: "No user found") { StatusCode = StatusCode.DoesNotExist };
            }

            return user;
        }
        public async Task SetRefreshTokenToUser(RefreshToken refreshToken, string email)
        {
            User? user = await _repository.User.GetUserByEmail(email);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;

            await _repository.User.Update(user);

            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }
        }

        public async Task InviteUserToColab(string invitedUserEmail, string currentUserEmail)
        {
            //Get users from data base
            User? currentUser = await _repository.User.GetUserByEmail(currentUserEmail);
            User? invitedUser = await _repository.User.GetUserByEmail(invitedUserEmail);

            //Check if user have colaborators
            if (currentUser.MyColaborators == null)
            {
                currentUser.MyColaborators = new List<User>();
            }

            //Add colaborator
            currentUser.MyColaborators.Add(invitedUser);

            //Try to save
            try
            {
                await _repository.User.Update(currentUser);
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }
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
