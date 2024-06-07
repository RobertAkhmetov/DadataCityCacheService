using DadataCityCacheService.Data;
using DadataCityCacheService.Models;
using DadataCityCacheService.Services.DadataApiClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<DadataApiPreferences>(
    builder.Configuration.GetSection(nameof(DadataApiPreferences)));

var preferences = builder.Configuration.GetSection(nameof(DadataApiPreferences))
    .Get<DadataApiPreferences>();

if (preferences == null)
{
    throw new ArgumentNullException("DadataApiPreferences should be set");
}
builder.Services.AddSingleton<IDadataApiClient, DadataApiClient>(x =>
    new DadataApiClient(preferences, x.GetRequiredService<ILogger<DadataApiClient>>()));


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 17))));

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());


builder.Configuration.AddJsonFile("appsettings.json");

var app = builder.Build();








app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
