using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            Farm? farm = new Farm();

            farm.FarmName = farmDto.FarmName;

            farm.MyUser = curentUser;

            curentUser.MyFarm = farm;

            _repository.Farm.Create(farm);

            _repository.User.Update(curentUser);

            _repository.Save();

            return Ok();
        }
    }
}
