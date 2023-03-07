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
        private readonly Services.Abstract.IAuthenticationService _authorization;
        private readonly IRepositoryManager _repository;
        private readonly IUserService _userService;
        
        private readonly IMapper _mapper;
        public AuthController(Services.Abstract.IAuthenticationService authorization, IRepositoryManager repository, IMapper mapper, IUserService userService)
        {
            _authorization = authorization;
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]   
        public async Task<ActionResult<string>> Login(UserDto dto)
        {
            Models.Enums.StatusCode status = _authorization.ValidateUser(dto.Password, dto.Email);

            switch (status)
            {
                case Models.Enums.StatusCode.WrongPassword:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("Wrong password")
                    { StatusCode = Models.Enums.StatusCode.WrongPassword }));
                case Models.Enums.StatusCode.DoesNotExist:
                    return BadRequest(JsonSerializer.Serialize(new CustomExeption("No user found")
                    { StatusCode = Models.Enums.StatusCode.DoesNotExist }));
            }

            string token = _authorization.CreateToken().Result;

            return Ok(token);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<string>> GetCurrentUser()
        {
            Models.Enums.StatusCode status = _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value, out User currentUser);

            if (status == Models.Enums.StatusCode.DoesNotExist)
            {
                return BadRequest(JsonSerializer.Serialize(new CustomExeption("No user found")
                { StatusCode = Models.Enums.StatusCode.DoesNotExist }));
            }

            return Ok(JsonSerializer.Serialize(_mapper.Map<UserDto>(currentUser)));
        }

       

    }
}
