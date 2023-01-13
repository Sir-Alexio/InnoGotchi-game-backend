using Microsoft.EntityFrameworkCore;
using InnoGotchi_backend.Models;

namespace InnoGotchi_backend.DataContext
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Pet> Pets { get; set; } = null!;
        public DbSet<Farm> Farms { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=InnoGotchi;Trusted_Connection=True");
        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Pet dog = new Pet("Dog");
            Pet cat = new Pet("Cat");
            Pet monkey = new Pet("Monkey");

            modelBuilder.Entity<Farm>().HasData(new Farm()
            {
                FarmId = 1,
                AlivePetsCount = 1,
                DeadPetsCount = 1,
                FarmName = "MyFarm",
                Pets = new List<Pet> { cat, monkey }
            }) ;

            modelBuilder.Entity<Farm>().HasData(new User() { Email = "mokharev@gmail.com",
                                                            FirstName = "Alexey",
                                                            LastName = "mokharev" });
        }
        */
    }
}
