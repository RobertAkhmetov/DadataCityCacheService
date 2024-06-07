using Dadata;
using Dadata.Model;
using DadataCityCacheService.Models;

namespace DadataCityCacheService.Services.DadataApiClient;

public class DadataApiClient : IDadataApiClient
{
    private readonly CleanClientAsync _apiClient;
    private readonly ILogger<DadataApiClient> _logger;

    public DadataApiClient(DadataApiPreferences preferences, ILogger<DadataApiClient> logger)
    {
        _logger = logger;
        _apiClient = new CleanClientAsync(preferences.Token, preferences.Secret);
    }

    public async Task<Address> GetAddress(string request)
    {
        try
        {
            return await _apiClient.Clean<Address>(request);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        return new Address();
    }
}