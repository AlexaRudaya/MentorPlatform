namespace Identity.API.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticsearchSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

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

        private static ElasticsearchSinkOptions ConfigureElasticsearchSink(IConfigurationRoot configuration,
            string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly()
                                         .GetName()
                                         .Name.ToLower()
                                         .Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            };
        }
    }
}