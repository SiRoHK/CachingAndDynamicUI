using CachingAndDynamicUI.Interfaces;
using CachingAndDynamicUI.Models;
using Newtonsoft.Json;

namespace CachingAndDynamicUI.Services
{
    public class JsonPlaceholderApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;
        private const string UsersKey = "users_key";

        public JsonPlaceholderApiService(HttpClient httpClient, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            _cacheService = cacheService;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _cacheService.GetOrSetAsync(UsersKey, async () =>
            {
                var response = await _httpClient.GetAsync("users");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(content);
            }, TimeSpan.FromMinutes(30));
        }
    }
}