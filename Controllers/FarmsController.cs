using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
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
        public async Task<IActionResult> CreateFarm(FarmDto farmDto)
        {
            bool isFarmCreated = await _farmService.CreateFarm(farmDto, User.FindFirst(ClaimTypes.Email).Value);

            if (!isFarmCreated)
            {
                return BadRequest("This farm name is already exist");
            }

            return Ok(JsonSerializer.Serialize(farmDto));
        }

        [HttpGet("current-farm")]
        [Authorize]
        public async Task<IActionResult> GetCurrentFarm()
        {
            //Get email from claims, after user authorization
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            //Get current farm
            Farm farm = await _farmService.GetFarm(email);

            FarmDto dto = new FarmDto();

            //Map to data transfer object
            dto = await Task.Run(() => _mapper.Map<FarmDto>(farm));

            return Ok(JsonSerializer.Serialize(dto));
        }
    }
}
