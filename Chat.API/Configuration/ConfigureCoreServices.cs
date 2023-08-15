using Chat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlServer(configuration.GetConnectionString("ChatConnection")));

            return services;
        }
    }
}