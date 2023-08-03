using Booking.Infrastructure.Consumer;
using Booking.Infrastructure.MessageBroker;
using MassTransit;
using MentorPlatform.Shared.MessageBus;
using Microsoft.Extensions.Options;

namespace Booking.API.Configuration
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
            services.AddEndpointsApiExplorer();
            services.AddFluentValidationAutoValidation();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services,
            IConfiguration configuration)
        {
            //var certificate = new X509Certificate2(@"E:\Projects\MentorPlatform\localhost.pfx", "password");
            //var key = new X509SecurityKey(certificate);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtBearerOptions =>
                    {
                        jwtBearerOptions.Authority = configuration["Authentication:Authority"];
                        jwtBearerOptions.Audience = configuration["Authentication:Audience"];

                        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
                        jwtBearerOptions.TokenValidationParameters.ValidIssuer = configuration["Authentication:Authority"];
                        //jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = key;

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

            services.AddAutoMapper(typeof(MapperInfrastructure));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IMentorBookingRepository, MentorBookingRepository>();
            services.AddScoped<IProducer, BookingProducer>();

            return services;
        }

        public static IServiceCollection ConfigureMessageBroker(this IServiceCollection services,
           IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<AvailabilityOfMentorEventConsumer>();

                busConfigurator.UsingRabbitMq((busRegistrationContext, busConfigurator) =>
                {
                    MessageBrokerSettings settings = busRegistrationContext.GetRequiredService<MessageBrokerSettings>();

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
    }
}