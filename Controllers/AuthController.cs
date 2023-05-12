using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InnoGotchi_backend.Services.Abstract;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authorizationService;
        private readonly IUserService _userService;

        
        private readonly IMapper _mapper;
        public AuthController(IAuthenticationService authorizationService,IMapper mapper, IUserService userService)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]   
        public async Task<IActionResult> Login(UserDto dto)
        {
            bool isUserValid = await _authorizationService.ValidateUser(dto.Password, dto.Email);
            //user validation
            if (!isUserValid)
            {
                return Unauthorized("Wrond password");
            }

            //create JWT token
            string token = await _authorizationService.CreateToken();

            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(JsonSerializer.Serialize(_mapper.Map<UserDto>(currentUser)));
        }
    }
}
