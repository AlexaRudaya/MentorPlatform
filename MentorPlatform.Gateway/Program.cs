using MentorPlatform.Gateway.Configuration;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOcelot(builder);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.UseOcelot();

app.Run();