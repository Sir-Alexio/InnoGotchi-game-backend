using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace InnoGotchi_backend.Repositories.Repository
{

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationContext db) : base(db) { }

        public async Task<User?> GetUserByEmail(string email)
        {
            User? user = await base.GetByCondition(s => s.Email == email, false).Result.FirstOrDefaultAsync();
            return user;
        }
        public async Task<User?> GetUserWithColaboratorsAsync(string email)
        {
            User? user = await _db.Users.Include(u=>u.MyColaborators).FirstOrDefaultAsync(u=>u.Email == email);
            return user;
        }

        public async override Task<IQueryable<User>> GetAll(bool trackChanges)
        {
            return _db.Users.Include(u => u.MyColaborators).AsQueryable();
        }
    }
}
