using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/farm")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public FarmsController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateFarm(FarmDto farmDto)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            User? curentUser = _repository.User.GetUserByEmail(email);

            if (curentUser == null)
            {
                return BadRequest("User is not authorize");
            }

            Farm farm = new Farm();

            farm.FarmName = farmDto.FarmName;

            curentUser.MyFarm = farm;

            _repository.User.Update(curentUser);

            _repository.Save();

            return Ok(JsonSerializer.Serialize(farmDto));
        }


        [HttpGet]
        [Authorize]
        public ActionResult GetCurrentFarm()
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            User? curentUser = _repository.User.GetUserByEmail(email);

            if (curentUser == null)
            {
                return BadRequest("User is not authorize");
            }

            Farm? farm = _repository.Farm.GetByCondition(x => x.UserId == curentUser.UserId, false).FirstOrDefault();
            
            FarmDto dto = new FarmDto();

            if (farm == null)
            {
                return Ok(dto);
            }

            dto.FarmName = farm.FarmName;

            dto.DeadPetsCount = farm.DeadPetsCount;
            
            dto.AlivePetsCount = farm.AlivePetsCount;

            return Ok(JsonSerializer.Serialize(dto));
        }
    }
}
