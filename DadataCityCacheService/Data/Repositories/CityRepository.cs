using Dadata.Model;
using DadataCityCacheService.Models;
using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService.Data.Repositories;

public class CityRepository : ICityRepository
{
    private readonly IAppDbContext _dbContext;

    public CityRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<City> GetCityByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cities.FirstOrDefaultAsync(x =>
            x.Result != null && x.Result.Contains(name.Trim()), cancellationToken) ?? new City();
    }

    public async Task<City?> GetCityByAddressAsync(Address address, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cities.FindAsync(new object?[] { address.fias_id }, cancellationToken)
               ?? await _dbContext.Cities.FindAsync(new object?[] { address.city_fias_id }, cancellationToken);
    }

    public async Task AddCityAsync(City city, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Cities
            .AnyAsync(e => e.FiasId == city.FiasId, cancellationToken);

        if (exists)
            return;

        await _dbContext.Cities.AddAsync(city, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}