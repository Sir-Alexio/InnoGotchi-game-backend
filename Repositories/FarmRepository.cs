using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Services;

namespace InnoGotchi_backend.Repositories
{
    public class FarmRepository:RepositoryBase<Farm>,IFarmRepository
    {
        public FarmRepository(ApplicationContext db) : base(db)
        {
        }
    }
}
