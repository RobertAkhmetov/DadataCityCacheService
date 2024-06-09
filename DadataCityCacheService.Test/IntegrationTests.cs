using DadataCityCacheService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;

namespace DadataCityCacheService.Test;
public class IntegrationTests
{
    private readonly HttpClient _client;
    private string _url = "/api/addresses";

    public IntegrationTests()
    {
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Addresses_GetAddressInfo_ShouldReturnCityOnlyInfo()
    {

        var content = new StringContent(JsonSerializer.Serialize("москва"), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(_url, content);

        Assert.True(response.IsSuccessStatusCode); // Проверяем, что статус-код успешен
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(responseContent); // Проверяем, что контент не пустой

        var responseCity = JsonSerializer.Deserialize(responseContent, typeof(City));




    }
}