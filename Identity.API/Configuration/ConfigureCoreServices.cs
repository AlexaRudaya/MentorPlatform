namespace Identity.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services,
            IConfiguration configuration,
            ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddSerilog(
            new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger());

            return services;
        }

        public static IServiceCollection ConfigureAPI(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
            {
                dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("IdentityConnection"),
                    NpgsqlOptionsAction);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services,
            IConfiguration configuration)
        {
            //var certificate = GetCertificate(configuration);

            var identityServerConfiguration = services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(configurationsStoreOptions =>
                {
                    configurationsStoreOptions.ResolveDbContextOptions = (serviceProvider, dbContextOptionsBuilder) =>
                    {
                        dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("IdentityServerConnection"),
                            NpgsqlOptionsAction);
                    };
                })
                .AddOperationalStore(operationStoreOptions =>
                {
                    operationStoreOptions.ResolveDbContextOptions = (serviceProvider, dbContextOptionsBuilder) =>
                    {
                        dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("IdentityServerConnection"),
                            NpgsqlOptionsAction);
                    };
                });
                //.AddSigningCredential(certificate);

            return services;
        }

        public static IServiceCollection ConfigureApplicationCore(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IValidator<LoginDto>, LoginValidator>();
            services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();

            return services;
        }

        private static void NpgsqlOptionsAction(NpgsqlDbContextOptionsBuilder npgsqlDbContextOptionsBuilder)
        {
            npgsqlDbContextOptionsBuilder.MigrationsAssembly("Identity.Infrastructure");
        }

        private static X509Certificate2 GetCertificate(IConfiguration configuration)
        {
            var certificatePath = configuration["Certificate:Path"];
            var certificatePassword = configuration["Certificate:Password"];

            return new X509Certificate2 (certificatePath!, certificatePassword);
        }
    }
}