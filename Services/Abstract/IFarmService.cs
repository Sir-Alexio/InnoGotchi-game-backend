using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IFarmService
    {
        public Task<bool> CreateFarm(FarmDto farmDto, string email);
        public Task<Farm> GetFarm(string email);
        public Task<bool> UpdateFarm(Farm farm);
    }
}
