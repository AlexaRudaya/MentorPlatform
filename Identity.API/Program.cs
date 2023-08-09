var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureLogging(builder)
    .ConfigureAPI()
    .ConfigureIdentity(builder.Configuration)
    .ConfigureIdentityServer(builder.Configuration)
    .ConfigureApplicationCore();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await IdentitySeed.SeedAsync(app);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();