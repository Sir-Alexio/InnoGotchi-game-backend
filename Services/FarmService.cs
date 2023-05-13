using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
using InnoGotchi_backend.Services.Abstract;
using InnoGotchi_backend.Models.Entity;

namespace InnoGotchi_backend.Services
{
    public class FarmService:IFarmService
    {
        private readonly IRepositoryManager _repository;
        public FarmService(IRepositoryManager repository)
        {
            _repository = repository;
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
