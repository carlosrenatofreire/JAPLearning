using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JAPLearning.Data.Contexts
{
    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlServer("Server=10.147.20.1\\SQLEXPRESS,64500;Database=JAPLearning;Trusted_Connection=True;TrustServerCertificate=True;");
            return new MainDbContext(optionsBuilder.Options);
        }
    }
}
