var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureLogging(builder)
    .ConfigureInfrastructure(builder.Configuration)
    .ConfigureApplicationCore()
    .ConfigureSignalR()
    .ConfigureCorePolicy();

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

app.MapHub<ChatHub>("/chatHub");

app.Run();