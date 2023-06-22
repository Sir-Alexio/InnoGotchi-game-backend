using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Repositories.Repository;

namespace InnoGotchi_backend.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationContext _db;
        private IUserRepository _userRepository;
        private IFarmRepository _farmRepository;
        private IPetRepository _petRepository;
        private IPetFeedingRepository _petFeedingRepository;
        private IPetDrinkingRepository _petDrinkingRepository;
        public RepositoryManager(ApplicationContext db)
        {
            _db = db;
        }

        public IUserRepository User {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_db);
                }
                return _userRepository;
            }
        
        }

        public IFarmRepository Farm
        {
            get
            {
                if (_farmRepository == null)
                {
                    _farmRepository = new FarmRepository(_db);
                }
                return _farmRepository;
            }

        }

        public IPetRepository Pet
        {
            get
            {
                if (_petRepository == null)
                {
                    _petRepository = new PetRepository(_db);
                }
                return _petRepository;
            }
        }

        public IPetFeedingRepository PetFeeding
        {
            get
            {
                if (_petFeedingRepository == null)
                {
                    _petFeedingRepository = new PetFeedingRepository(_db);
                }
                return _petFeedingRepository;
            }
        }

        public IPetDrinkingRepository PetDrinking
        {
            get
            {
                if (_petDrinkingRepository == null)
                {
                    _petDrinkingRepository = new PetDrinkingRepository(_db);
                }
                return _petDrinkingRepository;
            }
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
