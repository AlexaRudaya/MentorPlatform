var builder = WebApplication.CreateBuilder(args);

ConfigureCoreServices.ConfigureServices(builder.Configuration, builder.Services, builder.Logging);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<IdentitySeed>>();
logger.LogInformation("Seed data is running");

if (app.Environment.IsDevelopment())
{
    await IdentitySeed.SeedAsync(app);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityServer();

app.MapControllers();

app.Run();