using InnoGotchi_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.DTOs
{
    public class PetDto
    {
        public string PetName { get; set; }
        [Required]
        public DateTime AgeDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime LastHungerLevel { get; set; } = DateTime.Now;
        [Required] 
        public DateTime LastThirstyLevel { get; set; } = DateTime.Now;
        [Required]
        public int HappyDaysCount { get; set; } = 0;
        public string? Body { get; set; }
        public string? Eyes { get; set; }
        public string? Mouth { get; set; }
        public string? Nose { get; set; }

        public HungerLevel GetHungerLevel() 
        {
            int days = DateTime.Now.Subtract(LastHungerLevel).Days;

            if (days == 0)
            {
                return HungerLevel.Full;
            }
            else if (days == 1)
            {
                return HungerLevel.Normal;
            }
            else if (days == 2)
            {
                return HungerLevel.Hunger;
            }

            return HungerLevel.Dead;
        }

        public int GetAge()
        {
            return DateTime.Now.Subtract(this.AgeDate).Days/7;
        }

        public ThirstyLevel GetThirstyLevel()
        {
            int days = DateTime.Now.Subtract(this.LastThirstyLevel).Days;

            if (days == 0)
            {
                return ThirstyLevel.Full;
            }
            else if (days == 1)
            {
                return ThirstyLevel.Normal;
            }
            else if (days == 2)
            {
                return ThirstyLevel.Thirsty;
            }

            return ThirstyLevel.Dead;
        }
    }
}
