using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using DadataCityCacheService.Data;


namespace DadataCityCacheService.Test;
public class IntegrationTests
{
    private readonly HttpClient _client;
    private readonly AppDbContext _dbContext;
    private string _url = "/api/addresses";

    public IntegrationTests()
    {
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAddressInfo_WithCityName_ShouldReturnCityOnlyInfo()
    {
        List<string> cities = new()
        {
            "Москва",
            "Санкт-Петербург",
            "Новосибирск",
            "Екатеринбург",
            "Казань",
            "Нижний Новгород",
            "Челябинск",
            "Самара",
            "Омск",
            "Ростов-на-Дону",
            "Уфа",
            "Красноярск",
            "Воронеж",
            "Пермь",
            "Волгоград",
            "Краснодар",
            "Саратов",
            "Тюмень",
            "Ижевск",
            "Барнаул"
        };

        foreach (var city in cities)
        {
            var content = new StringContent(JsonSerializer.Serialize(city), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultResponseArray = JsonSerializer.Deserialize<string[]>(responseContent);

            Assert.NotEmpty(responseContent);
            Assert.True(response.IsSuccessStatusCode);

            Assert.True(resultResponseArray.Length == 5);
            Assert.All(resultResponseArray, item => Assert.NotNull(item));


            await Task.Delay(500);
        }
    }


    [Fact]
    public async Task GetAddressInfo_WithFullAddress_ShouldReturnAllFields()
    {
        List<string> addresses = new()
        {
            "Москва, Красная площадь, дом 1, кв. 1",
            "Санкт-Петербург, Невский проспект, дом 50, кв. 12",
            "Новосибирск, Красный проспект, дом 100, кв. 20",
            "Екатеринбург, проспект Ленина, дом 10, кв. 5",
            "Казань, улица Баумана, дом 5, кв. 15"
        };

        foreach (var address in addresses)
        {
            var content = new StringContent(JsonSerializer.Serialize(address), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultResponseArray = JsonSerializer.Deserialize<string[]>(responseContent);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(resultResponseArray.Length == 11);
            Assert.NotNull(resultResponseArray[7]);

            await Task.Delay(500);
        }
    }

    [Fact]
    public async Task GetAddressInfo_WithTrimmedCityNames_ShouldHandleProperly()
    {
        List<string> trimmedAddresses = new()
        {
            "вронеж",
            "спб, Невский проспект, дом 50, кв. 12",
            "Новосиб, Красный проспект, дом 100, кв. 20",
            "Екб",
            "Казан, улица Баумана, дом 5, кв. 15"
        };

        foreach (var str in trimmedAddresses)
        {
            var content = new StringContent(JsonSerializer.Serialize(str), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultResponseArray = JsonSerializer.Deserialize<string[]>(responseContent);

            Assert.NotEmpty(responseContent);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(resultResponseArray[1]);

            await Task.Delay(500);
        }
    }

    [Fact]
    public async Task GetAddressInfo_WithEmptyRequest_ShouldReturnEmptyValues()
    {
        
        var content = new StringContent(JsonSerializer.Serialize(string.Empty), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(_url, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var resultResponseArray = JsonSerializer.Deserialize<string[]>(responseContent);

        Assert.True(response.IsSuccessStatusCode);
        Assert.All(resultResponseArray, item => Assert.Null(item));
    }

}