using MundoDev.Mvc.Mappings;

namespace MundoDev.Mvc.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<DomainProfile>());

            return services;
        }
    }
}
