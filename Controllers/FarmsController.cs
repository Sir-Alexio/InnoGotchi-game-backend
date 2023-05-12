using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/farm")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmService _farmService;
        private readonly IMapper _mapper;

        public FarmsController(IFarmService farmService,IMapper mapper)
        {
            _farmService = farmService;
            _mapper = mapper;
        }

        [HttpPost("new-farm")]
        [Authorize]
        public ActionResult CreateFarm(FarmDto farmDto)
        {
            bool isFarmCreated = _farmService.CreateFarm(farmDto, User.FindFirst(ClaimTypes.Email).Value);

            if (!isFarmCreated)
            {
                return BadRequest("This farm name is already exist");
            }

            return Ok(JsonSerializer.Serialize(farmDto));
        }

        [HttpGet("current-farm")]
        [Authorize]
        public ActionResult GetCurrentFarm()
        {
            //Get email from claims, after user authorization
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            //Get current farm
            Farm farm = _farmService.GetFarm(email);

            FarmDto dto = new FarmDto();

            //Map to data transfer object
            dto = _mapper.Map<FarmDto>(farm);

            return Ok(JsonSerializer.Serialize(dto));
        }
    }
}
