using Microsoft.EntityFrameworkCore;
using JAPLearning.Data.Contexts;

namespace JAPLearning.Mvc.Configurations
{
    public static class ContextConfig
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["SQLSERVER__CONNECTIONSTRING"];

            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
