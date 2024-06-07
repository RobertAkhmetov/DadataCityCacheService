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

            var cachedInfoPresent = GetCachedInfo(address).ToArray() != null;

            

            if (Cities)
            {

                if (cachedInfoPresent)
                {
                    result = GetCachedInfo(address).ToArray();
                }

                if (cachedInfoPresent)
                {
                    result = GetCachedInfo(address).ToArray();
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
                result = GetCachedInfo(address).ToArray();
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


        public Cities GetCachedInfo(Address address)
        {
            var Cities = _context.Cities.FindAsync(address.fias_id);

            return Cities.Result;
        }


        public async Task<bool> CheckThisInfoCityOnly(Address address)
        {
            if (address.city_fias_id == null ||
                address.fias_id == null) return false;

            return address.city_fias_id.Equals(address.fias_id);
        }


        public async Task SaveToDb(Cities Cities)
        {
            bool exists = await _context.Cities
                .AnyAsync(e => e.FiasId == Cities.FiasId);

            if (exists) return;

            await _context.Cities.AddAsync(Cities);
            await _context.SaveChangesAsync();

        }

    }
}