using MentorPlatform.Gateway.Configuration;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOcelot(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();