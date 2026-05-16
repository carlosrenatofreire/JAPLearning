using Microsoft.EntityFrameworkCore;
using MundoDev.Data.Contexts;

namespace MundoDev.Mvc.Configurations
{
    public static class ContextConfig
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["CONNECTIONSTRINGS"];

            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
