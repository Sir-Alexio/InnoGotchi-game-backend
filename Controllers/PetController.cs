using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/pet")]
    [ApiController]
    public class PetController : ControllerBase
    {
        [HttpGet,Authorize]
        public async Task<ActionResult<string>> GetPet()
        {
            return Ok("Hello from api");
        }
    }
}
