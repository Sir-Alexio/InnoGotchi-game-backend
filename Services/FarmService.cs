﻿using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
using InnoGotchi_backend.Services.Abstract;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Models.DTOs;
using System.Security.Claims;

namespace InnoGotchi_backend.Services
{
    public class FarmService:IFarmService
    {
        private readonly IRepositoryManager _repository;
        private readonly IPetService _petService;
        public FarmService(IRepositoryManager repository, IPetService petService)
        {
            _repository = repository;
            _petService = petService;
        }

        public async Task<bool> CreateFarm(FarmDto farmDto, string email)
        {
            //Get current user for creating farm
            User? curentUser = await _repository.User.GetUserByEmail(email);

            if (curentUser == null)
            {
                throw new CustomExeption(message: "User does not exist") { StatusCode = StatusCode.DoesNotExist };
            }
            
            //Check if we already have farm with this name
            if (await _repository.Farm.GetByCondition(x=> x.FarmName == farmDto.FarmName,false).Result.FirstOrDefaultAsync() != null)
            {
                return false;
            }

            //If everything good we create new farm
            curentUser.MyFarm = new Farm() { FarmName = farmDto.FarmName };

            //Update current user cause we create farm for him
            try
            {
                await _repository.User.Update(curentUser);
                await _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public async Task<StatisticDto> GetFarmStatisticByEmail(string email)
        {
            //Create new statistic
            StatisticDto statistic = new StatisticDto();

            //Get pets by email
            List<Pet> pets = await _petService.GetAllPets(email);

            //If pets count zero - return empty statistics
            if (pets.Count == 0)
            {
                return statistic;
            }

            //Calculate alive pet count and dead pet count
            statistic.AlivePetCount = pets.Count(x => DateTime.Now.Subtract(x.LastHungerLevel).Days <= 2 && DateTime.Now.Subtract(x.LastThirstyLevel).Days <= 2);
            statistic.DeadPetCount = pets.Count(x => DateTime.Now.Subtract(x.LastHungerLevel).Days > 2 || DateTime.Now.Subtract(x.LastThirstyLevel).Days > 2);

            //Values for calculating average values
            int totalFeedDaysForPets = 0;
            int totalDrinkDaysForPets = 0;
            int totalHappyDaysCount = 0;
            int totalPetsAge = 0;

            foreach (Pet pet in pets)
            {
                //Local total values for calculating average feed and drinks day for one pet
                int totalFeedDays = 0;
                int totalDrinkDays = 0;

                //Get feedings and drinkings
                List<PetFeeding> feedings = pet.Feedings.ToList();
                List<PetDrinking> drinkings = pet.Drinkings.ToList();

                //Add age and happy days to total values
                totalPetsAge += DateTime.Now.Subtract(pet.AgeDate).Days / 7;
                totalHappyDaysCount += pet.HappyDaysCount;

                if (feedings.Count == 0 || drinkings.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < feedings.Count; i++)
                {
                    //Because value calculating between two lines in database
                    if (i == feedings.Count - 1)
                    {
                        break;
                    }

                    totalFeedDays += (int)feedings[i + 1].FeedDate.Subtract(feedings[i].FeedDate).TotalDays;
                }

                totalFeedDaysForPets += totalFeedDays / (feedings.Count - 1);

                for (int i = 0; i < drinkings.Count; i++)
                {
                    if (i == drinkings.Count - 1)
                    {
                        break;
                    }

                    totalDrinkDays += (int)drinkings[i + 1].DrinkDate.Subtract(drinkings[i].DrinkDate).TotalDays;
                }

                totalDrinkDaysForPets += totalDrinkDays / (drinkings.Count - 1);
            }

            //Calculating statistic
            statistic.AverageFeedingPeriod = totalFeedDaysForPets / pets.Count;
            statistic.AverageDrinkingPeriod = totalDrinkDaysForPets / pets.Count;
            statistic.AveragePetsAge = totalPetsAge / pets.Count;
            statistic.AverageHappyDaysCount = totalHappyDaysCount / pets.Count;

            return statistic;
        }
        public async Task<Farm> GetFarm(string email)
        {
            //Get current user for getting current farm
            User? curentUser = await _repository.User.GetUserByEmail(email);

            Farm? farm = await _repository.Farm.GetByCondition(x => x.UserId == curentUser.UserId, false).Result.FirstOrDefaultAsync();

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            return farm;
        }

        public async Task<Farm> GetFarmByName(string farmName)
        {
            //Get farm by farm Name
            Farm? farm = await _repository.Farm.GetByCondition(x => x.FarmName==farmName, false).Result.FirstOrDefaultAsync();

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            return farm;
        }

        public async Task<bool> UpdateFarm(Farm farm)
        {
            await _repository.Farm.Update(farm);

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
    }
}
