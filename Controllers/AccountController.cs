using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Enums;
using InnoGotchi_backend.Repositories.Abstract;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;


namespace InnoGotchi_backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {


        private readonly IUserService _userService;

        public AccountController( IUserService userService)
        {

            _userService = userService;
        }

        [Route("modify-user")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUser(UserDto dto)
        {
            await _userService.UpdateUser(dto);

            return Ok(JsonSerializer.Serialize(dto));
        }

        [Route("registration")]
        [HttpPost]
        public async Task<ActionResult<string>> Register(UserDto userDto)
        {
            await _userService.Registrate(userDto);

            return Ok();
        }

        [Route("change-password")]
        [HttpPatch]
        [Authorize]

        public async Task<ActionResult<string>> ChangePassword(ChangePasswordModel changePassword)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;

            StatusCode status = await _userService.ChangePassword(changePassword, email);

            switch (status)
            {
                case Models.Enums.StatusCode.WrongPassword:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Wrong password") { StatusCode = Models.Enums.StatusCode.WrongPassword }));
                    break;
                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database") { StatusCode = Models.Enums.StatusCode.WrongPassword }));
                    break;
            }

            return Ok();
        }
    }
}
