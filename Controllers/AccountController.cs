using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
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
        private readonly IRepositoryManager _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IRepositoryManager repository, IMapper mapper, IAuthenticationService authenticationService, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _authenticationService = authenticationService;
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

            await _userService.ChangePassword(changePassword, email);

            return Ok();
        }
    }
}
