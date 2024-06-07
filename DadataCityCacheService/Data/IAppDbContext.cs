using DadataCityCacheService.Models;
using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService.Data;

public interface IAppDbContext
{ 
    DbSet<Cities> Cities { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}