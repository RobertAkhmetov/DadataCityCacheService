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

        foreach(var city in cities)
        {
            var content = new StringContent(JsonSerializer.Serialize(city), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultResponseArray = JsonSerializer.Deserialize<string[]>(responseContent);

            Assert.NotEmpty(responseContent);
            Assert.True(response.IsSuccessStatusCode);
            //Assert.True(resultResponseArray.Length == 5);
            //Assert.True(!resultResponseArray.ToList<string>().Contains(null));

            await Task.Delay(500);

            



        }
    }




}