using Dadata.Model;
using DadataCityCacheService.Models;

namespace DadataCityCacheService.Extensions
{
    public static class Extensions
    {
        public static City GetCities(this Address address)
        {
            return new()
            {
                FiasId = address.fias_id,
                GeoLat = address.geo_lat,
                GeoLon = address.geo_lon,
                Result = address.result,
                Timezone = address.timezone
            };
        }
    }
}
