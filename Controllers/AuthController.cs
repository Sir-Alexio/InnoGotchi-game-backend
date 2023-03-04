using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Services.Abstract;
using InnoGotchi_backend.Repositories.Abstract;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Services.Abstract.IAuthenticationService _authorizationManager;
        private readonly IRepositoryManager _repository;
        
        private readonly IMapper _mapper;
        public AuthController(Services.Abstract.IAuthenticationService authorizationManager, IRepositoryManager repository, IMapper mapper)
        {
            _authorizationManager = authorizationManager;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpPost]   
        public async Task<ActionResult<string>> Login(UserDto dto)
        {
            if (!_authorizationManager.ValidateUser(dto).Result)
            {
                return BadRequest("Wrong email of password");
            }

            string token = _authorizationManager.CreateToken().Result;

            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<string>> GetCurrentUser()
        {
            UserDto dto = new UserDto();

            string? email = User.FindFirst(ClaimTypes.Email)?.Value;

            User? currentUser = _repository.User.GetUserByEmail(email);

            if (currentUser == null)
            {
                return BadRequest("User not found");
            }

            dto = _mapper.Map<UserDto>(currentUser);

            return Ok(JsonSerializer.Serialize(dto));
        }

       

    }
}
