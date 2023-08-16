using Chat.API.Configuration;
using Chat.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureCorePolicy()
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();