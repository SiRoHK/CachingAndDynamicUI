using CachingAndDynamicUI.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.IsNull)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        var item = await getItemCallback();

        if (item != null)
        {
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(item), expirationTime);
        }

        return item;
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}