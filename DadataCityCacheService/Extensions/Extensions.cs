using Dadata.Model;
using DadataCityCacheService.Models;

namespace DadataCityCacheService.Extensions
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
}
