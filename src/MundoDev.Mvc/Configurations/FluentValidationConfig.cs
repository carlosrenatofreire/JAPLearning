using FluentValidation;
using FluentValidation.AspNetCore;
using MundoDev.Mvc.Validators.Entities;

namespace MundoDev.Mvc.Configurations
{
    public static class FluentValidationConfig
    {
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();

            return services;
        }
    }
}
