using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Services.Abstract;
using System.Security.Claims;
using AutoMapper;
using System.Text.Json;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Services.LoggerService.Abstract;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/pet")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IFarmService _farmService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public PetController(IPetService petService, IFarmService farmService, IMapper mapper, ILoggerManager logger)
        {
            _petService = petService;
            _farmService = farmService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("current-pet/{petName}")]
        public async Task<IActionResult> GetCurrentPet(string petName)
        {
            Pet? pet = await _petService.GetCurrentPet(petName);

            if (pet == null)
            {
                _logger.LogInfo($"Can not find pet with name: {petName}");
                return BadRequest("Can not find pet");
            }

            return Ok(JsonSerializer.Serialize(_mapper.Map<PetDto>(pet)));
        }

        [HttpGet]
        [Authorize]
        [Route("all-pets")]
        public async Task<IActionResult> GetPets()
        {
            List<Pet>? pets = await _petService.GetAllPets(User.FindFirst(ClaimTypes.Email).Value);

            //Map to dtos list
            List<PetDto> dtos = _mapper.Map<List<PetDto>>(pets);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [HttpGet]
        [Authorize]
        [Route("foreign-all-pets/{farmName}")]
        public async Task<IActionResult> GetForeignPets(string farmName)
        {
            List<Pet>? pets = await _petService.GetAllPetsByFarm(farmName);

            //Map to dtos list
            List<PetDto> dtos = _mapper.Map<List<PetDto>>(pets);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [HttpPost]
        [Authorize]
        [Route("new-pet")]
        public async Task<IActionResult> CreatePet(PetDto dto)
        {
            //Get email from claims after user authorisation
            string? email = User.FindFirst(ClaimTypes.Email).Value;

            //Get current farm
            Farm currentFarm = await _farmService.GetFarm(email);

            //Map to pet
            Pet pet = _mapper.Map<Pet>(dto);

            pet.FarmId = currentFarm.FarmId;

            //Create pet
            bool isPetCreated = await _petService.CreatePet(pet);

            if (!isPetCreated)
            {
                _logger.LogInfo($"Can not create pet. This pet name: {pet.PetName}");
                return BadRequest("This pet name is already exist");
            }

            //Set alive pet counts +1 cause we created pet sucsessfuly
            currentFarm.AlivePetsCount += 1;

            bool isFarmUpdated = await _farmService.UpdateFarm(currentFarm);

            if (!isFarmUpdated)
            {
                _logger.LogInfo($"Can not update farm. Farm name: {currentFarm.FarmName}");
                return BadRequest("Can not update farm");
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("feed-current-pet")]
        public async Task<IActionResult> FeedCurrentPet(PetDto dto)
        {
            bool isPetFed =  await _petService.FeedPet(dto.PetName);

            if (!isPetFed)
            {
                _logger.LogInfo($"Can not Feed pet with name: {dto.PetName}");
                return BadRequest("Can not feed pet");
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("give-drink")]
        public async Task<IActionResult> GiveDrink(PetDto dto)
        {
            bool isPetDrunk = await _petService.GiveDrinkToPet(dto.PetName);
            
            if (!isPetDrunk)
            {
                _logger.LogInfo($"Can not give a drink to pet with name: {dto.PetName}");
                return BadRequest("Can not give a drink to pet");
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("pets")]
        public async Task<IActionResult> GetAllInnogotches()
        {
            List<Pet> pets = await _petService.GetAllInnogotches();

            List<PetDto> dto = _mapper.Map<List<PetDto>>(pets);

            return Ok(JsonSerializer.Serialize(dto));
        }
    }
}
