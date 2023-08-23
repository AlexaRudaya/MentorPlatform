using Ocelot.DependencyInjection;

namespace MentorPlatform.Gateway.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureOcelot(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddOcelot(builder.Configuration);

            builder.Services.AddSwaggerForOcelot(builder.Configuration);

            return services;
        }
    }
}