using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IPetService
    {
        public Task<bool> CreatePet(Pet pet);
        public Task<List<Pet>> GetAllPets(string email);
        public Task<Pet> GetCurrentPet(string petName);
        public Task<bool> FeedPet(string petName);
        public Task<bool> GiveDrinkToPet(string petName);
        public Task<List<Pet>> GetAllPetsByFarm(string farmName);
    }
}
