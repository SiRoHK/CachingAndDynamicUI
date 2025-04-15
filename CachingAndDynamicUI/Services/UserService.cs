using CachingAndDynamicUI.Interfaces;
using CachingAndDynamicUI.Models;
using Newtonsoft.Json;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly ICacheService _cacheService;
    private const string UsersKey = "users_data";

    public UserService(HttpClient httpClient, ICacheService cacheService)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
        _cacheService = cacheService;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _cacheService.GetOrSetAsync(UsersKey, FetchUsersFromApiAsync, TimeSpan.FromMinutes(10));
    }

    private async Task<List<User>> FetchUsersFromApiAsync()
    {
        var response = await _httpClient.GetAsync("users");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<User>>(content);
    }
}