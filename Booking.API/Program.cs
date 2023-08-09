var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureLogging(builder)
    .ConfigureAPI()
    .ConfigureAuthenticationAndAuthorization(builder.Configuration)
    .ConfigurePresentationService()
    .ConfigureCorePolicy()
    .ConfigureApplicationCore(builder.Configuration)
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureMessageBroker(builder.Configuration)
    .ConfigureHangfire(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();