using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IFarmService
    {
        public StatusCode CreateFarm(FarmDto farmDto, string email);
        public StatusCode GetFarm(string email, out Farm farm);
    }
}
