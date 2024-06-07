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


            await SaveToDb(address.GetCities());



            var Cities = await CheckThisInfoCityOnly(address);

            string[] result;

            var cachedInfoPresent = (await GetCachedInfo(address)).ToArray() != null;

            

            if (Cities)
            {

                if (cachedInfoPresent)
                {
                    result = (await GetCachedInfo(address)).ToArray();
                }
                else
                {
                    await SaveToDb(address.GetCities());
                    result = address.GetCities().ToArray();
                }

                return result;
            }


            if (cachedInfoPresent)
            {
                result = (await GetCachedInfo(address)).ToArray();
            }
            else
            {
                await SaveToDb(address.GetCities());
                result = address.GetCities().ToArray();
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

            concreteAddressData.AddRange(result);


            return concreteAddressData.ToArray();
        }


        public async Task<City> GetCachedInfo(Address address)
        {
            if (address is null) throw new NullReferenceException();

            var city = await _context.Cities.FindAsync(address.fias_id);

            return city;
        }


        public async Task<bool> CheckThisInfoCityOnly(Address address)
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