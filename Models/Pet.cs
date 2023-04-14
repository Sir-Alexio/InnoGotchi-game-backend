using InnoGotchi_backend.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    [Index(nameof(PetName), IsUnique = true)]
    public class Pet
    {
        [Key]
        public int PetId { get; set; }
        [Required]
        public string PetName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public HungerLevel HungerLevel { get; set; }
        [Required]
        public ThirstyLevel ThirstyLevel { get; set; }
        [Required]
        public int HappyDaysCount { get; set; } = 0;
        public string? Body { get; set; } 
        public string? Eyes { get; set; }
        public string? Mouth { get; set; }
        public string? Nose { get; set; }
        [Required]
        public int FarmId { get; set; }
        public virtual Farm Farm { get; set; }

    }
}
