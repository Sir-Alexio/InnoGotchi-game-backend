using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.Dto
{
    public class FarmDto
    {
        public int FarmId { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public int AlivePetsCount { get; set; } = 0;
        public int DeadPetsCount { get; set; } = 0;
        //public int? UserId { get; set; }
    }
}
