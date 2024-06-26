using Microsoft.AspNetCore.Mvc;
using Dadata.Model;
using DadataCityCacheService.Models;
using DadataCityCacheService.Services.DadataApiClient;
using Microsoft.EntityFrameworkCore;
using DadataCityCacheService.Extensions;
using DadataCityCacheService.Data;

namespace DadataCityCacheService.Controllers;

[ApiController]
[Route("api/addresses")]
public class AddressesController : ControllerBase
{
    private readonly IAppDbContext _context;
    private readonly IDadataApiClient _dadataApiClient;
    private readonly ILogger<AddressesController> _logger;
    public AddressesController(IAppDbContext context, IDadataApiClient dadataApiClient, ILogger<AddressesController> logger)
    {
        _context = context;
        _dadataApiClient = dadataApiClient;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<string[]>> GetAddressInfo([FromBody] string request)
    {
        string[] cityInfo;

        var address = await _dadataApiClient.GetAddress(request);

        if (address == default) return (await GetCachedInfoByCityName(request)).ToArray();


        var cachedInfo = await GetCachedInfo(address);

        if (IsCityInfoOnly(address))
        {

            if (cachedInfo != null)
            {
                cityInfo = cachedInfo.ToArray();
            }
            else
            {
                await SaveToDb(address.ToCity());
                cityInfo = address.ToCity().ToArray();
            }

            return cityInfo;
        }


        if (cachedInfo != null)
        {
            cityInfo = cachedInfo.ToArray();
        }
        else
        {
            cityInfo = address.ToCity().ToArray();
        }


        var concreteAddressData = new List<string>()
        {
            address.fias_id,
            address.region_with_type,
            address.city_with_type,
            address.street_with_type,
            address.house,
            address.flat
        };

        concreteAddressData.AddRange(cityInfo);


        return concreteAddressData.ToArray();
    }


    public async Task<City> GetCachedInfo(Address address)
    {
        if (address is null)
        {
            _logger.LogError("Address in null");
            return default;
        }

        try
        {
            var city = await _context.Cities.FindAsync(address.fias_id);

            if (city == null) city = await _context.Cities.FindAsync(address.city_fias_id);

            return city;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching cached info by address.");
        }

        return default;
    }

    public async Task<City> GetCachedInfoByCityName(string name)
    {
        try
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Result.Contains(name));

            return city;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching cached info by city name.");
        }


        return default;
    }


    public bool IsCityInfoOnly(Address address)
    {
        if (address.city_fias_id != null && address.fias_id != null)
        {
            return address.city_fias_id.Equals(address.fias_id);
        }

        return int.TryParse(address.fias_level, out int detailLevel) && detailLevel <= 4;
    }


    public async Task SaveToDb(City city)
    {
        bool exists = await _context.Cities
            .AnyAsync(e => e.FiasId == city.FiasId);

        if (exists) return;

        try
        {
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }
    }

}