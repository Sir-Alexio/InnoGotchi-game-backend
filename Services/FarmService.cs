using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Models.Enums;
using Microsoft.EntityFrameworkCore;
using InnoGotchi_backend.Services.Abstract;

namespace InnoGotchi_backend.Services
{
    public class FarmService:IFarmService
    {
        private readonly IRepositoryManager _repository;
        public FarmService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public bool CreateFarm(FarmDto farmDto, string email)
        {
            //Get current user for creating farm
            User? curentUser = _repository.User.GetUserByEmail(email);

            if (curentUser == null)
            {
                throw new CustomExeption(message: "User does not exist") { StatusCode = StatusCode.DoesNotExist };
            }
            
            //Check if we already have farm with this name
            if (_repository.Farm.GetByCondition(x=> x.FarmName == farmDto.FarmName,false).FirstOrDefault() != null)
            {
                return false;
            }

            //If everything good we create new farm
            curentUser.MyFarm = new Farm() { FarmName = farmDto.FarmName };

            //Update current user cause we create farm for him
            try
            {
                _repository.User.Update(curentUser);
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                throw new CustomExeption(message: "Can not update database") { StatusCode = StatusCode.UpdateFailed };
            }

            return true;
        }

        public Farm GetFarm(string email)
        {
            //Get current user for getting current farm
            User? curentUser = _repository.User.GetUserByEmail(email);

            Farm? farm = _repository.Farm.GetByCondition(x => x.UserId == curentUser.UserId, false).FirstOrDefault();

            if (farm == null)
            {
                throw new CustomExeption(message: "Farm does not exist") { StatusCode = StatusCode.DoesNotExist };
            }

            return farm;
        }

        public bool UpdateFarm(Farm farm)
        {
            _repository.Farm.Update(farm);

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
    }
}
