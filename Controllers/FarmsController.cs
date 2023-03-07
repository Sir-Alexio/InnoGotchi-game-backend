using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
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
        private readonly IFarmService _farmService;

        public FarmsController(IFarmService farmService)
        {
            _farmService = farmService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateFarm(FarmDto farmDto)
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            StatusCode status = _farmService.CreateFarm(farmDto, User.FindFirst(ClaimTypes.Email).Value);

            switch (status)
            {
                case Models.Enums.StatusCode.DoesNotExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("No user found")
                    { StatusCode = Models.Enums.StatusCode.DoesNotExist }));

                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));

                case Models.Enums.StatusCode.IsAlredyExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("This farm name is already exist!")
                    { StatusCode = Models.Enums.StatusCode.IsAlredyExist }));

            }

            return Ok(JsonSerializer.Serialize(farmDto));
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetCurrentFarm()
        {
            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            StatusCode status = _farmService.GetFarm(email,out Farm farm);

            if (status == Models.Enums.StatusCode.DoesNotExist)
            {
                return BadRequest(JsonSerializer.Serialize(new CustomExeption("No farm found!")
                { StatusCode = Models.Enums.StatusCode.IsAlredyExist }));
            }

            FarmDto dto = new FarmDto();

            dto.FarmName = farm.FarmName;

            dto.DeadPetsCount = farm.DeadPetsCount;
            
            dto.AlivePetsCount = farm.AlivePetsCount;

            return Ok(JsonSerializer.Serialize(dto));
        }
    }
}
