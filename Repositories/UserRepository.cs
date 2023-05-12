using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace InnoGotchi_backend.Repositories
{

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationContext db) : base(db) { }

        public async Task<User?> GetUserByEmail(string email)
        {
            User? user = await GetByCondition(s => s.Email == email, false).Result.FirstOrDefaultAsync();
            return user;
        } 
    }
}
