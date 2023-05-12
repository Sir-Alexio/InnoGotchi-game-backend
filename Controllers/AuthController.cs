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
        private readonly Services.Abstract.IAuthenticationService _authorizationService;
        private readonly IUserService _userService;

        
        private readonly IMapper _mapper;
        public AuthController(Services.Abstract.IAuthenticationService authorizationService,IMapper mapper, IUserService userService)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]   
        public async Task<ActionResult<string>> Login(UserDto dto)
        {
            //user validation
            if (!_authorizationService.ValidateUser(dto.Password, dto.Email))
            {
                return Unauthorized("Wrond password");
            }

            //create JWT token
            string token = _authorizationService.CreateToken().Result;

            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<string>> GetCurrentUser()
        {
            User currentUser = _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(JsonSerializer.Serialize(_mapper.Map<UserDto>(currentUser)));
        }
    }
}
