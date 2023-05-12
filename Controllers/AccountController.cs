using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;


namespace InnoGotchi_backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("modify-user")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUser(UserDto dto)
        {
            //Update User
            bool isUserUpdated = _userService.UpdateUser(dto);

            if (!isUserUpdated)
            {
                return BadRequest("No user found");
            }

            return Ok(JsonSerializer.Serialize(dto));
        }

        [Route("registration")]
        [HttpPost]
        public async Task<ActionResult<string>> Register(UserDto userDto)
        {
            bool isUserRegistrated = _userService.Registrate(userDto);

            if (!isUserRegistrated)
            {
                return BadRequest("This email is alredy exist");
            }

            return Ok();
        }

        [Route("change-password")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordModel changePassword)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;

            bool isPasswordChanged = _userService.ChangePassword(changePassword, email);

            if (!isPasswordChanged)
            {
                return Unauthorized("Password was incorrect");
            }

            return Ok();
        }
    }
}
