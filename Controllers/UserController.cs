using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userService.GetAll();

            List<UserDto> dtos = _mapper.Map<List<UserDto>>(users);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }
    }
}
