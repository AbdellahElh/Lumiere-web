using Rise.Client.Auth;
using Rise.Shared.Tenturncards;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml;

namespace Rise.Client.Tenturncards;

public class TenturncardService : ITenturncardService
{
    private readonly HttpClient _httpClient;

    public TenturncardService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    public async Task<List<TenturncardDto>> GetTenturncardsAsync()
    {
        var cards = await _httpClient.GetFromJsonAsync<List<TenturncardDto>>("Tenturncard");
        return cards!;
    }
    public async Task AddTenturncard(string tenturncardCode)
    {
        await _httpClient.PostAsJsonAsync($"Tenturncard/add/{tenturncardCode}", tenturncardCode);
    }

    public async Task UpdateTenturncardValueAsync(string activationCode)
    {
        await _httpClient.PostAsJsonAsync($"Tenturncard/update/{activationCode}", activationCode);
    }

    public async Task EditTenturncardAsync(TenturncardDto tenturncardDto)
    {
        await _httpClient.PostAsJsonAsync("Tenturncard/edit", tenturncardDto);
    }
}