using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InnoGotchi_backend.Services.Abstract;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;

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

            RefreshToken refreshToken = _authorizationService.CreateRefreshToken();

            SetRefreshToken(refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            Response.Cookies.Append("token", token, cookieOptions);

            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            User currentUser = await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(JsonSerializer.Serialize(_mapper.Map<UserDto>(currentUser)));
        }

        private void SetRefreshToken(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        }
    }
}
