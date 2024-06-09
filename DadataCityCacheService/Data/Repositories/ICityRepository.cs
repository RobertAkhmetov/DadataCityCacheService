using Dadata.Model;
using DadataCityCacheService.Models;

namespace DadataCityCacheService.Data.Repositories;

public interface ICityRepository
{
    Task<City> GetCityByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<City?> GetCityByAddressAsync(Address address, CancellationToken cancellationToken = default);

    Task AddCityAsync(City city, CancellationToken cancellationToken = default);
}