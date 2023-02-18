using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories;
using InnoGotchi_backend.Services;
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

namespace InnoGotchi_backend.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationManager _authorizationManager;
        private readonly IRepositoryManager _repository;
        public AuthController(IAuthenticationManager authorizationManager, IRepositoryManager repository)
        {
            _authorizationManager = authorizationManager;
            _repository = repository;
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

        [HttpGet("user/{token}")]
        //[Authorize]
        public async Task<ActionResult<string>> GetCurrentUser(string token)
        {
            UserDto dto = new UserDto();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken == null)
            {
                return BadRequest("User is not authorize!!!");
            }

            IEnumerable<Claim> claims = jwtToken.Claims;

            string? userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            
            User? currentUser = _repository.User.GetUserByEmail(userEmail);

            if (currentUser == null)
            {
                return BadRequest("User not found");
            }

            dto.UserName = currentUser.UserName;
            dto.FirstName = currentUser.FirstName;
            dto.LastName = currentUser.LastName;
            dto.Avatar = currentUser.Avatar;
            dto.Email = currentUser.Email;
            dto.Password = "password";

            return Ok(JsonSerializer.Serialize(dto));
        }

       

    }
}
