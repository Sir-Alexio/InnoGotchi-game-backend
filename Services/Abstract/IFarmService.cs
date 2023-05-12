using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IFarmService
    {
        public bool CreateFarm(FarmDto farmDto, string email);
        public Farm GetFarm(string email);
        public bool UpdateFarm(Farm farm);
    }
}
