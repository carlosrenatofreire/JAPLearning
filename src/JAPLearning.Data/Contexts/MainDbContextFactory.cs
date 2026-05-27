using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JAPLearning.Data.Contexts
{
    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS")
                ?? throw new InvalidOperationException("Variável de ambiente CONNECTIONSTRINGS não definida.");

            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new MainDbContext(optionsBuilder.Options);
        }
    }
}
