using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Authorization;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Services.Abstract;
using System.Security.Claims;
using AutoMapper;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/pet")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IFarmService _farmService;
        private readonly IMapper _mapper;
        public PetController(IPetService petService, IFarmService farmService, IMapper mapper)
        {
            _petService = petService;
            _farmService = farmService;
            _mapper = mapper;

        }

        [HttpGet]
        [Authorize]
        [Route("current-pet/{petName}")]
        public async Task<ActionResult<string>> GetCurrentPet(string petName)
        {
            Pet? pet = _petService.GetCurrentPet(petName);

            if (pet == null)
            {
                return BadRequest("Can not find pet");
            }

            return Ok(JsonSerializer.Serialize(_mapper.Map<PetDto>(pet)));
        }

        [HttpGet]
        [Authorize]
        [Route("all-pets")]
        public async Task<ActionResult<string>> GetPets()
        {
            List<Pet>? pets = _petService.GetAllPets(User.FindFirst(ClaimTypes.Email).Value);

            //Map to dtos list
            List<PetDto> dtos = _mapper.Map<List<PetDto>>(pets);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [HttpPost]
        [Authorize]
        [Route("new-pet")]
        public ActionResult CreatePet(PetDto dto)
        {
            //Get email from claims after user authorisation
            string email = User.FindFirst(ClaimTypes.Email).Value;

            //Get current farm
            Farm currentFarm = _farmService.GetFarm(email);

            //Map to pet
            Pet pet = _mapper.Map<Pet>(dto);

            pet.FarmId = currentFarm.FarmId;

            //Create pet
            bool isPetCreated = _petService.CreatePet(pet);

            if (!isPetCreated)
            {
                return BadRequest("This pet name is already exist");
            }

            //Set alive pet counts +1 cause we created pet sucsessfuly
            currentFarm.AlivePetsCount += 1;

            bool isFarmUpdated = _farmService.UpdateFarm(currentFarm);

            if (!isFarmUpdated)
            {
                return BadRequest("Can not update farm");
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("feed-current-pet")]
        public IActionResult FeedCurrentPet(PetDto dto)
        {
            bool isPetFed =  _petService.FeedPet(dto.PetName);

            if (!isPetFed)
            {
                return BadRequest("Can not feed pet");
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("give-drink")]
        public IActionResult GiveDrink(PetDto dto)
        {
            bool isPetDrunk = _petService.GiveDrinkToPet(dto.PetName);
            
            if (!isPetDrunk)
            {
                return BadRequest("Can not give a drink to pet");
            }

            return Ok();
        }
    }
}
