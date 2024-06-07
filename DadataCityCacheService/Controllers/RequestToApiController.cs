using Microsoft.AspNetCore.Mvc;
using Dadata;
using Dadata.Model;
using System.Text;
using DadataCityCacheService.Models;

namespace DadataCityCacheService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadaCacheController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string[]>> Get([FromBody] string request)
        {
            var token = "055fa6c2af4562aeee76ada479f00b2218610bd0";
            var secret = "2a984b6d1c9211c0ba9f209148a5f73f0a7c4a34";

            var api = new CleanClientAsync(token, secret);

            var address = await api.Clean<Address>(request);


            if (!await CheckResponseCorrect()) throw new();




            var cityInfoOnly = await CheckThisInfoCityOnly(address);

            string[] result;

            if (cityInfoOnly)
            {
                result = TryGetCachedInfo(address, out CityInfoOnly cachedInfo) ? cachedInfo.ToArray() : new string[]
                    {
                    "dfsf"
                    };

                return result;
            }







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

        public bool TryGetCachedInfo(Address address, out CityInfoOnly cityInfoOnly)
        {
            cityInfoOnly = new()
            {

            };

            return true;
        }


        public async Task<bool> CheckThisInfoCityOnly(Address address)
        {
            return address.city_fias_id.Equals(address.fias_id);
        }

    }
}