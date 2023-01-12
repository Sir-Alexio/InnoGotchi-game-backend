using System.ComponentModel.DataAnnotations.Schema;

namespace InnoGotchi_backend.Models
{
    public class Farm
    {
        public int FarmId { get; set; }
        public string FarmName { get; set; }
        public int AlivePetsCount { get; set; }
        public int DeadPetsCount { get; set; }
        public List<Pet> Pets { get; set; }
        public int UserId{ get; set; }
    }
}
