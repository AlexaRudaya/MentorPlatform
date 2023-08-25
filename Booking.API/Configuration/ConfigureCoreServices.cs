namespace Booking.API.Configuration
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
            services.AddOptions();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddFluentValidationAutoValidation();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services,
            IConfiguration configuration)
        {
            var certificate = new X509Certificate2("/https/mentorplatform.pfx", "password");
            var key = new X509SecurityKey(certificate);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtBearerOptions =>
                    {
                        jwtBearerOptions.Authority = configuration["Authentication:Authority"];
                        jwtBearerOptions.Audience = configuration["Authentication:Audience"];

                        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
                        jwtBearerOptions.TokenValidationParameters.ValidIssuer = configuration["Authentication:Authority"];
                        jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = key;

                        jwtBearerOptions.Configuration = new OpenIdConnectConfiguration();
                        jwtBearerOptions.RequireHttpsMetadata = false;
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                       .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                       .RequireAuthenticatedUser()
                       .Build());
            });

            return services;
        }

        public static IServiceCollection ConfigurePresentationService(this IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Booking.API",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            });

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                    "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                swaggerGenOptions.EnableAnnotations();
            });

            return services;
        }

        public static IServiceCollection ConfigureCorePolicy(this IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureApplicationCore(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MentorApiOptions>(configuration.GetSection("MentorAPI"));

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IValidator<StudentCreateDto>, StudentValidator>();
            services.AddScoped<IValidator<BookingDto>, BookingValidator>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingForMentorService, BookingForMentorService>();
            services.AddScoped<IGetMentorClient, GetMentorClient>();

            return services;
        }

        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BookingDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlServer(configuration.GetConnectionString("BookingConnection")));

            services.AddDbContext<HangfireDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlServer(configuration.GetConnectionString("HangfireConnection")));

            services.AddAutoMapper(typeof(MapperInfrastructure));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IMentorBookingRepository, MentorBookingRepository>();
            services.AddScoped<IProducer, BookingProducer>();
            services.AddScoped<IBackgroundJobsService, BackgroundJobsService>();

            return services;
        }

        public static IServiceCollection ConfigureMessageBroker(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<AvailabilityOfMentorEventConsumer>();

                busConfigurator.UsingRabbitMq((busRegistrationContext, busConfigurator) =>
                {
                    MessageBrokerSettings settings = busRegistrationContext.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                    busConfigurator.Host(new Uri(settings.Host), hostConfigurator =>
                    {
                        hostConfigurator.Username(settings.Username);
                        hostConfigurator.Password(settings.Password);
                    });

                    busConfigurator.ConfigureEndpoints(busRegistrationContext);
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureHangfire(this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true,
                EnableHeavyMigrations = true,
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            };

            services.AddHangfire(globalConfiguration => globalConfiguration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), options));

            services.AddHangfireServer();

            return services;
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