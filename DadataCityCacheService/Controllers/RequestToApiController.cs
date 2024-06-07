using Microsoft.AspNetCore.Mvc;

using Dadata;
using Dadata.Model;
using System.Net;

namespace DadataCityCacheService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadaCacheController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DadaCacheController> _logger;

        public DadaCacheController(ILogger<DadaCacheController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] string request)
        {
            var token = "055fa6c2af4562aeee76ada479f00b2218610bd0";
            var secret = "2a984b6d1c9211c0ba9f209148a5f73f0a7c4a34";

            var api = new CleanClientAsync(token, secret);
            // or any of the following, depending on the API method
            //api = new SuggestClientAsync(token);
            //api = new OutwardClientAsync(token);
            //api = new ProfileClientAsync(token, secret);

            var address = await api.Clean<Address>(request);






            return Ok(
                address.fias_id + "," +
                address.region_with_type + "," +
                address.city_with_type + "," +
                address.street_with_type + "," +
                address.house + "," +
                address.flat
                ); 
        }
    }
}