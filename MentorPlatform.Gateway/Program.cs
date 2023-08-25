var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOcelot(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = app.Configuration.GetSection("OcelotOptions:PathToSwaggerGenerator").Value;
});

await app.UseOcelot();

app.Run();