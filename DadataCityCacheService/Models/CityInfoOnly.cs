using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DadataCityCacheService.Models
{
    [Table("cities")]
    public class City
    {
        [Key]
        [Column("fias_id")]
        public string FiasId { get; set; } = null!;

        [Column("geo_lat")]
        public string? GeoLat { get; set; }

        [Column("geo_lon")]
        public string? GeoLon { get; set; }

        [Column("result")]
        public string? Result { get; set; }

        [Column("timezone")]
        public string? Timezone { get; set; }

        public string[] ToArray()
        {
            return new[] { FiasId, GeoLat, GeoLon, Result, Timezone };
        }
    }
}
