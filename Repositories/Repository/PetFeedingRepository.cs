using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;

namespace InnoGotchi_backend.Repositories.Repository
{
    public class PetFeedingRepository : RepositoryBase<PetFeeding>,IPetFeedingRepository
    {
        public PetFeedingRepository(ApplicationContext db) : base(db)
        {
        }
    }
}
