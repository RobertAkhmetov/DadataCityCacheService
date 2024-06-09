using DadataCityCacheService;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace DadataCityCacheService;

public class Program
{
    public static void Main(string[] args)
    {
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

    }

}