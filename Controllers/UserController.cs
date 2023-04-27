using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("all-users")]
        public IActionResult GetAllUsers()
        {
            List<User> users = new List<User>();

            StatusCode status = _userService.GetAll(out users);

            switch (status)
            {
                case Models.Enums.StatusCode.DoesNotExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("No farm found for this user")
                    { StatusCode = Models.Enums.StatusCode.DoesNotExist }));
            }

            List<UserDto> dtos = _mapper.Map<List<UserDto>>(users);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }
    }
}
