using Doppler.Extensions.Configuration;

namespace JAPLearning.Mvc.Configurations
{
    public static class DopplerConfig
    {
        public static WebApplicationBuilder AddDopplerConfiguration(this WebApplicationBuilder builder)
        {
            // Token lido de variável de ambiente DOPPLER_TOKEN (nunca hardcoded)
            var token = Environment.GetEnvironmentVariable("DOPPLER_TOKEN_JAPLEARNING");

            if (string.IsNullOrWhiteSpace(token))
            {
                // Em desenvolvimento local sem Doppler configurado, continua sem ele
                // (appsettings.json ou variáveis locais serão usadas)
                return builder;
            }

            builder.Configuration.AddDoppler(options =>
            {
                options.ServiceToken = token;
                options.Project      = "jap-learning";
                options.Config       = builder.Environment.IsProduction() ? "prd" : "dev_personal";
            });

            var root = (IConfigurationRoot)builder.Configuration;
            root.Reload();

            return builder;
        }
    }
}
