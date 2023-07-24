﻿namespace Mentors.API.Configuration
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
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services,
            IConfiguration configuration)
        {
            var certificate = new X509Certificate2(@"E:\Projects\MentorPlatform\localhost.pfx", "password");
            var key = new X509SecurityKey(certificate);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                          .AddJwtBearer(jwtBearerOptions =>
                          {
                              jwtBearerOptions.Authority = configuration["Authentication:Authority"];
                              jwtBearerOptions.Audience = configuration["Authentication:Audience"];

                              jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
                              jwtBearerOptions.TokenValidationParameters.ValidIssuer = configuration["Authentication:Authority"];
                              jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = key;
                              ;

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
            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Mentors.API",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            });

            services.AddSwaggerGen(_ =>
            {
                _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                    "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                _.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                _.EnableAnnotations();
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

        public static IServiceCollection ConfigureApplicationCore(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddScoped<IValidator<CategoryDto>, CategoryValidator>();
            services.AddScoped<IValidator<AvailabilityDto>, AvailabilityValidator>();
            services.AddScoped<IValidator<MentorCreateDto>, MentorValidator>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();

            return services;
        }
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MentorDbContext>(_ => _
             .UseSqlServer(configuration.GetConnectionString("MentorsConnection")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

            return services;
        }
    }
}