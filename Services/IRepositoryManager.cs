using InnoGotchi_backend.Repositories;

namespace InnoGotchi_backend.Services
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        void Save();
    }
}
