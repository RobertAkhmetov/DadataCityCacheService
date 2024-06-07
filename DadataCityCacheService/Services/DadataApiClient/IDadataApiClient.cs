using Dadata.Model;

namespace DadataCityCacheService.Services.DadataApiClient;

public interface IDadataApiClient
{
    Task<Address> GetAddress(string request);
}