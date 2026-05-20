using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JAPLearning.Data.Contexts
{
    public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLSERVER__CONNECTIONSTRING")
                ?? throw new InvalidOperationException("Variável de ambiente SQLSERVER__CONNECTIONSTRING não definida.");

            var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new MainDbContext(optionsBuilder.Options);
        }
    }
}
