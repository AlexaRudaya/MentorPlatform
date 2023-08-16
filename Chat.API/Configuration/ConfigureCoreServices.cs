using Chat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureCorePolicy(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader();
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

        public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
        { 
            services.AddSignalR();

            return services;
        }
    }
}