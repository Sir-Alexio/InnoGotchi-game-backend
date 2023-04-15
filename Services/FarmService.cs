using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
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

        public StatusCode CreateFarm(FarmDto farmDto, string email)
        {
            User? curentUser = _repository.User.GetUserByEmail(email);

            if (curentUser == null)
            {
                return StatusCode.DoesNotExist;
            }
            else if (_repository.Farm.GetByCondition(x=> x.FarmName == farmDto.FarmName,false).FirstOrDefault() != null)
            {
                return StatusCode.IsAlredyExist;
            }

            curentUser.MyFarm = new Farm() { FarmName = farmDto.FarmName };

            _repository.User.Update(curentUser);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }

            return StatusCode.Ok;
        }

        public StatusCode GetFarm(string email,out Farm farm)
        {
            User? curentUser = _repository.User.GetUserByEmail(email);

            farm = _repository.Farm.GetByCondition(x => x.UserId == curentUser.UserId, false).FirstOrDefault();

            if (farm == null)
            {
                return StatusCode.DoesNotExist;
            }

            return StatusCode.Ok;
        }

        public StatusCode UpdateFarm(Farm farm)
        {
            _repository.Farm.Update(farm);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateException)
            {
                return StatusCode.UpdateFailed;
            }

            return StatusCode.Ok;
        }
    }
}
