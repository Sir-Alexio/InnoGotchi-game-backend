using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public RegistrationController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpPost("registration")]
        public OkResult Register(User requestUser, string password)
        {
            CreatePasswortHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            requestUser.Password = passwordHash;
            requestUser.PasswordSalt = passwordSalt;

            _db.Users.Add(requestUser);
            _db.SaveChanges();

            return Ok();
        }

        private void CreatePasswortHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
