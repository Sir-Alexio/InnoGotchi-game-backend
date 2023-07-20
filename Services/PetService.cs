using AutoMapper;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using InnoGotchi_backend.Services.LoggerService.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_backend.Services
{
    public class PetService : IPetService
    {
        private readonly IRepositoryManager _repository;
        private readonly IFarmService _farmSevice;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public PetService(IRepositoryManager repository, IFarmService farmService, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _farmSevice = farmService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Pet>> GetAllInnogotches()
        {
            List<Pet> pets = new List<Pet>();

            //Get current farm with user 

            pets = (await _repository.Pet.GetAll(trackChanges: false)).ToList();

            return pets;
        }
        private async Task CalculateHappyDaysCount(List<Pet> pets)
        {
            //For each pet in list we calculate happy days count
            foreach (Pet pet in pets)
            {
                //Check if we already calculate happy days count today
                if (DateTime.Now.Subtract(pet.LastHappyDaysCountUpdated).Days < 1) { continue; }

                //Check if pet alive
                if (DateTime.Now.Subtract(pet.LastHungerLevel).Days >= 2 && DateTime.Now.Subtract(pet.LastThirstyLevel).Days >= 2) { continue; }

                //Update last happe days count updated value
                pet.LastHappyDaysCountUpdated = DateTime.Now;

                pet.HappyDaysCount += 1;

                await _repository.Pet.Update(pet);
            }

            await _repository.Save();
        }


        public async Task<Pet> GetCurrentPet(string petName)
        {
            //Get current pet by name
            Pet? pet = (await _repository.Pet.GetByCondition(x => x.PetName == petName, false)).FirstOrDefault();

            if (pet == null)
            {
                _logger.LogInfo($"Pet with name: {petName} does not exist.");
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            return pet;
        }
        public async Task<bool> CreatePet(Pet pet)
        {
            if (pet == null)
            {
                _logger.LogInfo($"Pet does not exist. No instrance of pet in createPet method.");
                throw new CustomExeption(message: "Pet does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            //Check if we already have pet with this name
            if ((await _repository.Pet.GetByCondition(x => x.PetName == pet.PetName, false)).FirstOrDefault() != null)
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
                _logger.LogError("Can not update database. Save method in pet service.");
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
                _logger.LogInfo($"No farm found with user email:{email}");
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            pets = await _repository.Pet.GetAllPetsForFarmStatistic(farm.FarmId);

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
                _logger.LogInfo($"No farm found with farm name:{farmName}");
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            pets = (await _repository.Pet.GetByCondition(x => x.FarmId == farm.FarmId, false)).ToList();

            await CalculateHappyDaysCount(pets);

            return pets;
        }

        public async Task<bool> FeedPet(string petName)
        {
            //Get pet by name
            Pet pet = (await _repository.Pet.GetByCondition(x => x.PetName == petName, true)).First();

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
                _logger.LogError("Can not update database. Save method in pet service.");
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<bool> GiveDrinkToPet(string petName)
        {
            Pet pet = (await _repository.Pet.GetByCondition(x => x.PetName == petName, true)).First();

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
                _logger.LogError("Can not update database. Save method in pet service.");
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        //Each feed store in datadase
        private async Task SaveFeedInformation(Pet pet)
        {
            PetFeeding petFeeding = new PetFeeding();

            petFeeding.FeedDate = DateTime.Now;

            petFeeding.MyPet = pet;

            await _repository.PetFeeding.Create(petFeeding);
        }

        //Each drink store in datadase
        private async Task SaveDrinkInformation(Pet pet)
        {
            PetDrinking petDrinking = new PetDrinking();

            petDrinking.DrinkDate = DateTime.Now;

            petDrinking.MyPet = pet;

            await _repository.PetDrinking.Create(petDrinking);
        }
    }
}
