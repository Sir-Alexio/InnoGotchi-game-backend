using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IAuthenticationManager _authenticationManager;
        public AccountController(IRepositoryManager repository, IMapper mapper, IAuthenticationManager authenticationManager)
        {
            _repository = repository;
            _mapper = mapper;
            _authenticationManager = authenticationManager;
        }

        [HttpPatch]
        [Authorize]
        public async Task<ActionResult<string>> UpdateUser(UserDto dto)
        {
            User? user = _repository.User.GetUserByEmail(dto.Email);

            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Avatar = dto.Avatar;

            _repository.User.Update(user);

            _repository.Save();

            dto = _mapper.Map<UserDto>(user);

            return Ok(JsonSerializer.Serialize(dto));
        }


    }
}
