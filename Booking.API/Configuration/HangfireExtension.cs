namespace Booking.API.Configuration
{
    public static class HangfireExtension
    {
        public static IServiceCollection MigrateHangfire(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            using (var hangfireContext = scope.ServiceProvider.GetService<HangfireDbContext>())
            {
                hangfireContext.Database.Migrate();
            }

            return services;
        }
    }
}