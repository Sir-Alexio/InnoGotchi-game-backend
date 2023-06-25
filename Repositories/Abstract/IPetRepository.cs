using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_backend.Repositories.Abstract
{
    public interface IPetRepository:IRepositoryBase<Pet>
    {
        public Task<List<Pet>> GetAllPetsForFarmStatistic(int farmId);
    }
}
