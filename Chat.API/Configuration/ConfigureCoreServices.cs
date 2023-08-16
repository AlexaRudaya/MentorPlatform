using Chat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
        {
            services.AddSignalR();

            return services;
        }

        public static IServiceCollection ConfigureCorePolicy(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins("http://localhost:7006")
                                     .AllowAnyHeader()
                                     .WithMethods("GET", "POST")
                                     .SetIsOriginAllowed((host) => true)
                                     .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlServer(configuration.GetConnectionString("ChatConnection")));

            return services;
        }
    }
}