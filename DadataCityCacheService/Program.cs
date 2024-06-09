using DadataCityCacheService;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Load Kestrel configuration from appsettings.json
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();