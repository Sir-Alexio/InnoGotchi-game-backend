using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models
{
    public class Pet
    {
        [Key]
        public int PetId { get; set; }
        [Required]
        public string PetName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string HungerLevel { get; set; }
        [Required]
        public string ThirstyLevel { get; set; }
        [Required]
        public int HappyDaysCount { get; set; } = 0;
       
        public Pet(string petName)
        {
            this.PetName = petName;
            Age = 5;
            HungerLevel = "FULL";
            ThirstyLevel = "NORMAL";
            HappyDaysCount = 1;
        }
       
    }
}
