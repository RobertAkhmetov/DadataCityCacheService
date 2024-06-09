using DadataCityCacheService.Data;
using DadataCityCacheService.Data.Repositories;
using DadataCityCacheService.Models;
using DadataCityCacheService.Services.DadataApiClient;
using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException("DefaultConnection should be set");

        services.Configure<DadataApiPreferences>(configuration.GetSection(nameof(DadataApiPreferences)));

        var preferences = configuration.GetSection(nameof(DadataApiPreferences))
            .Get<DadataApiPreferences>();

        if (preferences == null || string.IsNullOrWhiteSpace(preferences.Secret)
                                || string.IsNullOrWhiteSpace(preferences.Token))
            throw new ArgumentNullException("DadataApiPreferences should be set");

        services.AddSingleton<IDadataApiClient, DadataApiClient>(x =>
            new DadataApiClient(preferences, x.GetRequiredService<ILogger<DadataApiClient>>()));

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 17))));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }
}