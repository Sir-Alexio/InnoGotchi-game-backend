using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Entity;
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
        public PetService(IRepositoryManager repository, IFarmService farmService, IMapper mapper)
        {
            _repository = repository;
            _farmSevice = farmService;
            _mapper = mapper;
        }

        public async Task<List<Pet>> GetAllInnogotches()
        {
            List<Pet> pets = new List<Pet>();

            //Get current farm with user 

            pets = await _repository.Pet.GetAll(trackChanges: false).Result.ToListAsync();

            return pets;
        }
        private async Task CalculateHappyDaysCount(List<Pet> pets)
        {
            foreach (Pet pet in pets)
            {
                if (DateTime.Now.Subtract(pet.LastHappyDaysCountUpdated).Days < 1) { continue; }

                if (DateTime.Now.Subtract(pet.LastHungerLevel).Days >= 2 && DateTime.Now.Subtract(pet.LastThirstyLevel).Days >= 2) { continue; }

                pet.LastHappyDaysCountUpdated = DateTime.Now;

                pet.HappyDaysCount += 1;

                await _repository.Pet.Update(pet);
            }

            await _repository.Save();
        }


        public async Task<Pet> GetCurrentPet(string petName)
        {
            //Get current pet by name
            Pet? pet = await _repository.Pet.GetByCondition(x => x.PetName == petName, false).Result.FirstOrDefaultAsync();

            if (pet == null)
            {
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            return pet;
        }
        public async Task<bool> CreatePet(Pet pet)
        {
            if (pet == null)
            {
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            //Check if we already have pet with this name
            if (await _repository.Pet.GetByCondition(x => x.PetName == pet.PetName, false).Result.FirstOrDefaultAsync() != null)
            {
                return false;
            }

            await _repository.Pet.Create(pet);

            try
            {
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<List<Pet>> GetAllPets(string email)
        {
            //Create new pet list
            List<Pet> pets = new List<Pet>();

            //Get current farm with user email
            Farm? farm = await _farmSevice.GetFarm(email);

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            pets = await _repository.Pet.GetByCondition(x => x.FarmId == farm.FarmId, false).Result.ToListAsync();

            await CalculateHappyDaysCount(pets);

            return pets;
        }

        public async Task<List<Pet>> GetAllPetsByFarm(string farmName)
        {
            //Create new pet list
            List<Pet> pets = new List<Pet>();

            //Get current farm with user email
            Farm? farm = await _farmSevice.GetFarmByName(farmName);

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            pets = await _repository.Pet.GetByCondition(x => x.FarmId == farm.FarmId, false).Result.ToListAsync();

            await CalculateHappyDaysCount(pets);

            return pets;
        }

        public async Task<bool> FeedPet(string petName)
        {
            //Get pet by name
            Pet pet = await _repository.Pet.GetByCondition(x => x.PetName == petName, true).Result.FirstAsync();

            //Set hunger level to DateTime now
            pet.LastHungerLevel = DateTime.Now;

            await SaveFeedInformation(pet);
            try
            {
                await _repository.Pet.Update(pet);
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> GiveDrinkToPet(string petName)
        {
            Pet pet = await _repository.Pet.GetByCondition(x => x.PetName == petName, true).Result.FirstAsync();

            //Set Thirsty level to current time
            pet.LastThirstyLevel = DateTime.Now;

            await SaveDrinkInformation(pet);

            try
            {
                await _repository.Pet.Update(pet);
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        private async Task SaveFeedInformation(Pet pet)
        {
            PetFeeding petFeeding = new PetFeeding();

            petFeeding.FeedDate = DateTime.Now;

            petFeeding.MyPet = pet;

            await _repository.PetFeeding.Create(petFeeding);
        }

        private async Task SaveDrinkInformation(Pet pet)
        {
            PetDrinking petDrinking = new PetDrinking();

            petDrinking.DrinkDate = DateTime.Now;

            petDrinking.MyPet = pet;

            await _repository.PetDrinking.Create(petDrinking);
        }
    }
}
