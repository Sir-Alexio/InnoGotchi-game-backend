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

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("modify-user")]
        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUser(UserDto dto)
        {
            StatusCode status = _userService.UpdateUser(dto);

            if (status == Models.Enums.StatusCode.UpdateFailed)
            {
                return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database")
                { StatusCode = Models.Enums.StatusCode.UpdateFailed }));
            }

            return Ok(JsonSerializer.Serialize(dto));
        }

        [Route("registration")]
        [HttpPost]
        public async Task<ActionResult<string>> Register(UserDto userDto)
        {
            StatusCode status = _userService.Registrate(userDto);

            switch (status)
            {
                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database")
                    { StatusCode = Models.Enums.StatusCode.UpdateFailed }));

                case Models.Enums.StatusCode.IsAlredyExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("This email is alredy exist!")
                    { StatusCode = Models.Enums.StatusCode.IsAlredyExist }));
            }

            return Ok();
        }

        [Route("change-password")]
        [HttpPatch]
        [Authorize]

        public async Task<ActionResult<string>> ChangePassword(ChangePasswordModel changePassword)
        {
            string email = User.FindFirst(ClaimTypes.Email).Value;

            StatusCode status = _userService.ChangePassword(changePassword, email);

            switch (status)
            {
                case Models.Enums.StatusCode.WrongPassword:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Wrong password")
                    { StatusCode = Models.Enums.StatusCode.WrongPassword }));

                case Models.Enums.StatusCode.UpdateFailed:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Can not update database")
                    { StatusCode = Models.Enums.StatusCode.WrongPassword }));
            }

            return Ok();
        }
    }
}
