using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Models;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private ApplicationContext _db;

        public PetController(ApplicationContext db)
        {
            _db = db;
        }

        public IEnumerable<Pet> GetPets()
        {
            return _db.Pets.ToList();
        }
    }
}
