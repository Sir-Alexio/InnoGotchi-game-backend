using InnoGotchi_backend.Models;
using InnoGotchi_backend.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InnoGotchi_backend.Services
{
    public class AuthenticationService : Controller, Abstract.IAuthenticationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;

        private static User? _user;
        public AuthenticationService(IRepositoryManager repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(string password,string email)
        {
            _user = await _repository.User.GetUserByEmail(email);

            if (_user == null)
            {
                throw new CustomExeption(message: "No user found") { StatusCode = Models.Enums.StatusCode.DoesNotExist };
            }

            if (!VerifyPasswordHash(password, _user.Password, _user.PasswordSalt))
            {
                return false;
            }

            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));

            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, _user.Email)
            };

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");

            JwtSecurityToken tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires:
            DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
            signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

    }
}
