using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnoGotchi_backend.Models
{
    [Index(nameof(FarmName), IsUnique = true)]
    public class Farm
    {
        [Key]
        public int FarmId { get; set; }
        [Required]
        public string FarmName { get; set; }
        [Required]
        public int AlivePetsCount { get; set; }
        [Required]
        public int DeadPetsCount { get; set; }
        public virtual ICollection<Pet>? Pets { get; set; }
        public int UserId { get; set; }
        public virtual User? MyUser { get; set; }

    }
}
