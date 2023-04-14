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
        [HttpGet,Authorize]
        public async Task<ActionResult<string>> GetPet()
        {
            return Ok("Hello from api");
        }

        [HttpPost]
        [Authorize]
        [Route("new-pet")]
        public ActionResult CreatePet(PetDto dto)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;

            _farmService.GetFarm(email,out var currentFarm);

            Pet pet = new Pet();

            _mapper.Map(dto, pet);

            pet.Farm = currentFarm;
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
            _farmService.GetFarm(email, out var currentFarm1);
            currentFarm1.AlivePetsCount += 1;

            _farmService.UpdateFarm(currentFarm1);

            return Ok();
        }
    }
}
