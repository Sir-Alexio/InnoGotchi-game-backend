using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.Entity
{
    [Index(nameof(PetId), IsUnique = false)]
    public class PetDrinking
    {
        [Key]
        public int DrinkId { get; set; }
        [Required]
        public DateTime DrinkDate { get; set; }
        [Required]
        public int PetId { get; set; }
        public virtual Pet MyPet { get; set; }
    }
}
