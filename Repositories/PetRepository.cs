using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;

namespace InnoGotchi_backend.Repositories
{
    public class PetRepository : RepositoryBase<Pet>, IPetRepository
    {
        public PetRepository(ApplicationContext db) : base(db)
        {
        }
    }
}
