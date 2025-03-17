using Rise.Shared.Accounts;
using System.Net.Http.Json;

namespace Rise.Client.Account.services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;
    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task AddUserAsync(RegisterDto registerDto)
    {
        await _httpClient.PostAsJsonAsync("account/register",registerDto);
    }
}