using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_backend.Repositories.Repository
{
    public class PetRepository : RepositoryBase<Pet>, IPetRepository
    {
        public PetRepository(ApplicationContext db) : base(db)
        {
        }

        public async Task<List<Pet>> GetAllPetsForFarmStatistic(int farmId)
        {
            return _db.Pets.Where(x=>x.FarmId == farmId).Include(x=>x.Drinkings).Include(x=>x.Feedings).ToList();
        }
    }
}
