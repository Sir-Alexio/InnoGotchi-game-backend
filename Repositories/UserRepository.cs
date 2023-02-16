using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace InnoGotchi_backend.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationContext db) : base(db)
        {
        }

        public User? GetUserByEmail(string email)
        {
            User? user = GetByCondition(s => s.Email == email, false).FirstOrDefault();
            return user;
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
