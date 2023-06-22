using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
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
        private readonly IUserService _userService;
        private readonly IPetService _petService;
        private readonly IMapper _mapper;

        public FarmsController(IFarmService farmService,IMapper mapper, IUserService userService, IPetService petService)
        {
            _farmService = farmService;
            _mapper = mapper;
            _userService = userService;
            _petService = petService;
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

        [Authorize]
        [HttpGet("foreign-farm/{email}")]
        public async Task<IActionResult> GetForeignFarm(string email)
        {
            List<User> iAmCollab = await _userService.GetUsersIAmCollab(User.FindFirst(ClaimTypes.Email)?.Value);

            User foreignUser = await _userService.GetUser(email);

            if (foreignUser == null) { return NotFound(); }

            if (!iAmCollab.Contains(foreignUser)) { return Unauthorized(); }

            Farm foreignFarm = await _farmService.GetFarm(email);

            FarmDto dto = _mapper.Map<FarmDto>(foreignFarm);

            return Ok(JsonSerializer.Serialize(dto));
        }

        [Authorize]
        [HttpGet]
        [Route("statistic")]
        public async Task<IActionResult> GetFarmStatistic()
        {
            StatisticDto statistic = new StatisticDto();

            List<Pet> pets = await _petService.GetAllPets(User.FindFirst(ClaimTypes.Email)?.Value);

            statistic.AlivePetCount = pets.Count(x => DateTime.Now.Subtract(x.LastHungerLevel).Days <= 2 && DateTime.Now.Subtract(x.LastThirstyLevel).Days <= 2);
            statistic.DeadPetCount = pets.Count(x => DateTime.Now.Subtract(x.LastHungerLevel).Days > 2 || DateTime.Now.Subtract(x.LastThirstyLevel).Days > 2);

            foreach (Pet pet  in pets)
            {
                int totalFeedDays = 0;
                int totalDrinkDays = 0;

                List<PetFeeding> feedings = pet.Feedings.ToList();
                List<PetDrinking> drinking = pet.Drinkings.ToList();

                for (int i = 0; i < feedings.Count; i++)
                {
                    if (i == feedings.Count-1)
                    {
                        break;
                    }

                    totalFeedDays += (int)feedings[i + 1].FeedDate.Subtract(feedings[i].FeedDate).TotalDays;
                }

                for (int i = 0; i < drinking.Count; i++)
                {
                    if (i == drinking.Count - 1)
                    {
                        break;
                    }

                    totalDrinkDays += (int)drinking[i + 1].DrinkDate.Subtract(drinking[i].DrinkDate).TotalDays;
                }
            }
            //To do farm statistic

            return Ok(JsonSerializer.Serialize(statistic));

        }
    }
}
