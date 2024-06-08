using Microsoft.AspNetCore.Mvc;
using Dadata.Model;
using DadataCityCacheService.Models;
using DadataCityCacheService.Services.DadataApiClient;
using Microsoft.EntityFrameworkCore;
using DadataCityCacheService.Extensions;
using DadataCityCacheService.Data;

namespace DadataCityCacheService.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly IAppDbContext _context;
        private readonly IDadataApiClient _dadataApiClient;
        public AddressesController(IAppDbContext context, IDadataApiClient dadataApiClient)
        {
            _context = context;
            _dadataApiClient = dadataApiClient;
        }

        [HttpGet]
        public async Task<ActionResult<string[]>> GetAddressInfo([FromBody] string request)
        {
            var address = await _dadataApiClient.GetAddress(request);

            
            string[] cityInfo;

            var cachedInfo = await GetCachedInfo(address);

            if (await IsCityInfoOnly(address))
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
            if (address is null) throw new NullReferenceException();

            var city = await _context.Cities.FindAsync(address.fias_id);

            if (city == null) city = await _context.Cities.FindAsync(address.city_fias_id);

            return city;
        }


        public async Task<bool> IsCityInfoOnly(Address address)
        {
            if (address.city_fias_id == null ||
                address.fias_id == null) return false;

            return address.city_fias_id.Equals(address.fias_id);
        }


        public async Task SaveToDb(City city)
        {
            bool exists = await _context.Cities
                .AnyAsync(e => e.FiasId == city.FiasId);

            if (exists) return;

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

        }

    }
}