var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureLogging(builder.Configuration, builder.Logging)
    .ConfigureAPI()
    .ConfigureAuthenticationAndAuthorization(builder.Configuration)
    .ConfigurePresentationService()
    .ConfigureCorePolicy()
    .ConfigureApplicationCore()
    .ConfigureInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await MentorsSeed.SeedAsync(app);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();