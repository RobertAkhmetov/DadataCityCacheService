using Microsoft.AspNetCore.Mvc;
using Dadata;
using Dadata.Model;
using System.Text;
using DadataCityCacheService.Models;
using System;

namespace DadataCityCacheService.Controllers
{
    public static class Extensions
    {
        public static CityInfoOnly GetCityInfoOnly(this Address address)
        {
            return new()
            {
                fias_id = address.fias_id,
                geo_lat = address.geo_lat,
                geo_lon = address.geo_lon,
                result = address.result,
                timezone = address.timezone
            };
        }
    }


    [ApiController]
    [Route("[controller]")]
    public class DadaCacheController : ControllerBase
    {
        AppDbContext _context;
        public DadaCacheController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<string[]>> Get([FromBody] string request)
        {
            var token = "055fa6c2af4562aeee76ada479f00b2218610bd0";
            var secret = "2a984b6d1c9211c0ba9f209148a5f73f0a7c4a34";

            var api = new CleanClientAsync(token, secret);

            var address = await api.Clean<Address>(request);


            if (!await CheckResponseCorrect()) throw new();


            await SaveToDb(address.GetCityInfoOnly());



            var cityInfoOnly = await CheckThisInfoCityOnly(address);

            string[] result;

            var cachedInfoPresent = GetCachedInfo(address).ToArray() != null;

            

            if (cityInfoOnly)
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
                    await SaveToDb(address.GetCityInfoOnly());
                    result = address.GetCityInfoOnly().ToArray();
                }

                return result;
            }


            if (cachedInfoPresent)
            {
                result = GetCachedInfo(address).ToArray();
            }
            else
            {
                await SaveToDb(address.GetCityInfoOnly());
                result = address.GetCityInfoOnly().ToArray();
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






            //if(await CheckThisInfoCityOnly(address))
            //{
            //    result.Add(address.fias_id);
            //    result.Add(address.result);
            //    result.Add(address.geo_lat);
            //    result.Add(address.geo_lon);
            //    result.Add(address.timezone);
            //}
            //else
            //{
            //    result =
            //        new()
            //        {
            //            address.fias_id,
            //            address.region_with_type,
            //            address.city_with_type,
            //            address.street_with_type,
            //            address.house,
            //            address.flat,
            //        };
            //}






            return Ok(
                new string[]
                {
                    "fdsfd"
                }
                //address.fias_id + "," +
                //address.region_with_type + "," +
                //address.city_with_type + "," +
                //address.street_with_type + "," +
                //address.house + "," +
                //address.flat
                ); 
        }

        public async Task<bool> CheckResponseCorrect()
        {
            return true;
        }

        public CityInfoOnly GetCachedInfo(Address address)
        {
            var cityInfoOnly = new CityInfoOnly();

            return cityInfoOnly;
        }


        public async Task<bool> CheckThisInfoCityOnly(Address address)
        {
            return address.city_fias_id.Equals(address.fias_id);
        }


        public async Task SaveToDb(CityInfoOnly cityInfoOnly)
        {
            await _context.cities.AddAsync(cityInfoOnly);
            await _context.SaveChangesAsync();

        }

    }
}