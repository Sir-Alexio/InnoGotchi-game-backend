using Microsoft.EntityFrameworkCore;
using InnoGotchi_backend.Models;

namespace InnoGotchi_backend.DataContext
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Pet> Pets { get; set; } = null!;
        public DbSet<Farm> Farms { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=InnoGotchi;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>().HasOne(u => u.MyFarm)
                .WithOne(a => a.MyUser)
                .HasForeignKey<Farm>(a => a.UserId);

            model.Entity<Pet>().HasOne(e => e.Farm)
                .WithMany(d => d.Pets)
                .HasForeignKey(e => e.FarmId);

            model.Entity<User>().HasMany(s => s.MyColaborators)
                .WithMany(c => c.IAmColaborator)
                .UsingEntity(j => j.ToTable("UserColab"));
        }
    }
}
