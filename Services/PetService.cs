using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_backend.Services
{
    public class PetService : IPetService
    {
        private readonly IRepositoryManager _repository;
        private readonly IFarmService _farmSevice;
        private readonly IMapper _mapper;
        public PetService(IRepositoryManager repository, IFarmService farmService,IMapper mapper)
        {
            _repository = repository;
            _farmSevice = farmService;
            _mapper = mapper;
        }
        public Pet GetCurrentPet(string petName)
        {
            //Get current pet by name
            Pet? pet = _repository.Pet.GetByCondition(x => x.PetName == petName, false).FirstOrDefault();

            if (pet == null) 
            { 
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist }; 
            }

            return pet;
        }
        public bool CreatePet(Pet pet)
        {
            if (pet == null)
            {
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist };
            }
            
            //Check if we already have pet with this name
            if (_repository.Pet.GetByCondition(x => x.PetName == pet.PetName, false).FirstOrDefault() != null)
            {
                return false;
            }

            _repository.Pet.Create(pet);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public List<Pet> GetAllPets(string email)
        {
            //Create new pet list
            List<Pet> pets = new List<Pet>();

            //Get current farm with user email
            Farm? farm = _farmSevice.GetFarm(email);

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            pets = _repository.Pet.GetByCondition(x=>x.FarmId == farm.FarmId,false).ToList();

            return pets;
        }

        public bool FeedPet(string petName)
        {
            //Get pet by name
            Pet pet = _repository.Pet.GetByCondition(x => x.PetName == petName, true).First();

            //Set hunger level to DateTime now
            pet.LastHungerLevel = DateTime.Now;

            try
            {
                _repository.Pet.Update(pet);
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public bool GiveDrinkToPet(string petName)
        {
            Pet pet = _repository.Pet.GetByCondition(x => x.PetName == petName, true).First();

            //Set Thirsty level to current time
            pet.LastThirstyLevel = DateTime.Now;

            try
            {
                _repository.Pet.Update(pet);
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }
    }
}
