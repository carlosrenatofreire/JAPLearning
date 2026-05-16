using Doppler.Extensions.Configuration;

namespace MundoDev.Mvc.Configurations
{
    public static class DopplerConfig
    {
        public static WebApplicationBuilder AddDopplerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddDoppler(options =>
            {
                options.ServiceToken = "dp.st.dev_carlos.VJxtuLmTSW0akkPfes2Wc95GceAk4Z5iDvS9yejGVWE";
                options.Project = "mundodev";
                options.Config = builder.Environment.IsProduction() ? "prd" : "dev_carlos";
            });

            var root = (IConfigurationRoot)builder.Configuration;
            root.Reload();

            return builder;
        }
    }
}
