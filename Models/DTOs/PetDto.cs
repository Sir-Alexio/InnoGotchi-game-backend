using InnoGotchi_backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.DTOs
{
    public class PetDto
    {
        public string PetName { get; set; }
        [Required]
        public DateTime AgeDate { get; set; }
        [Required]
        public DateTime LastHungerLevel { get; set; }
        [Required]
        public DateTime LastThirstyLevel { get; set; }
        [Required]
        public int HappyDaysCount { get; set; } = 0;
        public string? Body { get; set; }
        public string? Eyes { get; set; }
        public string? Mouth { get; set; }
        public string? Nose { get; set; }

        public HungerLevel GetHungerLevel() 
        {
            int minutes = DateTime.Now.Subtract(LastHungerLevel).Minutes;

            if (minutes == 0)
            {
                return HungerLevel.Full;
            }
            else if (minutes == 1)
            {
                return HungerLevel.Normal;
            }
            else if (minutes == 2)
            {
                return HungerLevel.Hunger;
            }

            return HungerLevel.Dead;
        }

        public int GetAge()
        {
            return DateTime.Now.Subtract(this.AgeDate).Minutes;
        }

        public ThirstyLevel GetThirstyLevel()
        {
            int minutes = DateTime.Now.Subtract(this.LastThirstyLevel).Minutes;

            if (minutes == 0)
            {
                return ThirstyLevel.Full;
            }
            else if (minutes == 1)
            {
                return ThirstyLevel.Normal;
            }
            else if (minutes == 2)
            {
                return ThirstyLevel.Thirsty;
            }

            return ThirstyLevel.Dead;
        }
    }
}
