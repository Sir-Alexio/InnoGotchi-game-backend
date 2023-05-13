using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;

namespace InnoGotchi_backend.Repositories.Repository
{
    public class PetRepository : RepositoryBase<Pet>, IPetRepository
    {
        public PetRepository(ApplicationContext db) : base(db)
        {
        }
    }
}
