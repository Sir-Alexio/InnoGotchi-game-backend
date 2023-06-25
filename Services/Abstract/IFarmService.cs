using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IFarmService
    {
        public Task<bool> CreateFarm(FarmDto farmDto, string email);
        public Task<Farm> GetFarm(string email);
        public Task<bool> UpdateFarm(Farm farm);
        public Task<Farm> GetFarmByName(string farmName);
        public Task<StatisticDto> GetFarmStatisticByEmail(string email)

    }
}
