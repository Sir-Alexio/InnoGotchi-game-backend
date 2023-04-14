using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.DTOs
{
    public class PetDto
    {
        public string PetName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string HungerLevel { get; set; }
        [Required]
        public string ThirstyLevel { get; set; }
        [Required]
        public int HappyDaysCount { get; set; } = 0;
        public string? Body { get; set; }
        public string? Eyes { get; set; }
        public string? Mouth { get; set; }
        public string? Nose { get; set; }
    }
}
