namespace Identity.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static void ConfigureServices(IConfiguration configuration,
            IServiceCollection services,
            ILoggingBuilder logging)
        {
            #region Logger

            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger());

            #endregion

            #region API Configuration

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            #endregion

            #region Mapper

            services.AddAutoMapper(typeof(MapperProfile));

            #endregion

            #region Identity Configuration

            services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
            {
                dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("IdentityConnection"),
                    NpgsqlOptionsAction);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            #endregion

            #region IdentityServer Configuration

            var certificate = GetCertificate(configuration);

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
                })
                .AddSigningCredential(certificate);

            #endregion

            #region Services

            services.AddScoped<IAccountService, AccountService>();

            #endregion

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