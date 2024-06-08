using DadataCityCacheService.Models;
using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<City> Cities => Set<City>();

    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}

