using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Enums;

namespace InnoGotchi_backend.Services.Abstract
{
    public interface IPetService
    {
        public StatusCode CreatePet(Pet pet);
        public StatusCode GetAllPets(string email, out List<Pet> pets);
        public StatusCode GetCurrentPet(string petName, out Pet? pet);
    }
}
