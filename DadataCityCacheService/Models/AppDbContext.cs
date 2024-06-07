using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService.Models;

public class AppDbContext : DbContext
{
        public DbSet<CityInfoOnly> cities { get; set; }

    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}

