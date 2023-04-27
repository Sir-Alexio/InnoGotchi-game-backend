using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Authorization;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Services.Abstract;
using System.Security.Claims;
using InnoGotchi_backend.Models.Enums;
using AutoMapper;
using System.Text.Json;
using System.ComponentModel.Design;

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
            StatusCode status = _petService.GetCurrentPet(petName, out Pet? pet);

            if(status == Models.Enums.StatusCode.DoesNotExist) {
                return BadRequest(JsonSerializer.Serialize(new CustomExeption("No pet found.")
                { StatusCode = Models.Enums.StatusCode.DoesNotExist }));
            }

            return Ok(JsonSerializer.Serialize(_mapper.Map<PetDto>(pet)));
        }

        [HttpGet]
        [Authorize]
        [Route("all-pets")]
        public async Task<ActionResult<string>> GetPets()
        {
            StatusCode status = _petService.GetAllPets(User.FindFirst(ClaimTypes.Email).Value, out List<Pet>? pets);
            switch (status)
            {
                case Models.Enums.StatusCode.DoesNotExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("No farm found for this user")
                    { StatusCode = Models.Enums.StatusCode.DoesNotExist }));
            }

            List<PetDto> dtos = _mapper.Map<List<PetDto>>(pets);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [HttpPost]
        [Authorize]
        [Route("new-pet")]
        public ActionResult CreatePet(PetDto dto)
        {
            Farm currentFarm = new Farm();

            string email = User.FindFirst(ClaimTypes.Email).Value;

            _farmService.GetFarm(email,out currentFarm);

            Pet pet = new Pet();

            _mapper.Map(dto, pet);

            pet.FarmId = currentFarm.FarmId;

            StatusCode status = _petService.CreatePet(pet);

            switch (status)
            {
                case Models.Enums.StatusCode.DoesNotExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("No Pet found")
                    { StatusCode = Models.Enums.StatusCode.DoesNotExist }));

                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));

                case Models.Enums.StatusCode.IsAlredyExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("This pet name is already exist!")
                    { StatusCode = Models.Enums.StatusCode.IsAlredyExist }));
            }
            
            currentFarm.AlivePetsCount += 1;

            status =  _farmService.UpdateFarm(currentFarm);

            switch (status)
            {
                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update farm table in database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));
            }

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("feed-current-pet")]
        public IActionResult FeedCurrentPet(PetDto dto)
        {
            StatusCode status = _petService.FeedPet(dto.PetName);

            switch (status)
            {
                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update pet table in database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));

                case Models.Enums.StatusCode.InsertDuplicateValue:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Invalid duplicated key")
                    { StatusCode = Models.Enums.StatusCode.InsertDuplicateValue }));
            }
            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("give-drink")]
        public IActionResult GiveDrink(PetDto dto)
        {
            StatusCode status = _petService.GiveDrinkToPet(dto.PetName);

            switch (status)
            {
                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update pet table in database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));

                case Models.Enums.StatusCode.InsertDuplicateValue:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Invalid duplicated key")
                    { StatusCode = Models.Enums.StatusCode.InsertDuplicateValue }));
            }
            return Ok();
        }
    }
}
