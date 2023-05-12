using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IPetService
    {
        public bool CreatePet(Pet pet);
        public List<Pet> GetAllPets(string email);
        public Pet GetCurrentPet(string petName);
        public bool FeedPet(string petName);
        public bool GiveDrinkToPet(string petName);
    }
}
