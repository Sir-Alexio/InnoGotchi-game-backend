using InnoGotchi_backend.Models;

namespace InnoGotchi_backend.Services
{
    public interface IUserRepository:IRepositoryBase<User>
    {
        public User? GetUserByEmail(string email);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
