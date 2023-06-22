using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_backend.Models.Entity
{
    [Index(nameof(PetId), IsUnique = false)]
    public class PetFeeding
    {
        [Key]
        public int FeedId { get; set; }
        [Required]
        public DateTime FeedDate { get; set; }
        [Required]
        public int PetId { get; set; }
        public virtual Pet MyPet { get; set; }

    }
}
