using Microsoft.EntityFrameworkCore;

namespace DadataCityCacheService.Models
{
    [PrimaryKey(propertyName: "fias_id")]
    public class CityInfoOnly
    {
        public string fias_id { get; set; }

        public string geo_lat { get; set; }

        public string geo_lon { get; set; }

        public string result { get; set; }

        public string timezone { get; set; }

        public string[] ToArray()
        {
            return new[] { fias_id, geo_lat, geo_lon, result, timezone };
        }
    }
}
