using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_backend.DataContext
{
    public class AppContext:DbContext
    {
        public AppContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=(localdb)\\MSSQLLocalDB;database=InnoGotchi;Trusted_Connection=true");
            Console.WriteLine("Complete");
        }

    }
}
