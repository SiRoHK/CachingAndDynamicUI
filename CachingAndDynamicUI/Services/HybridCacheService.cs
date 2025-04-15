using CachingAndDynamicUI.Interfaces;
using CachingAndDynamicUI.Services;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

public class HybridCacheService : ICacheService
{
    private readonly RedisCacheService _redisCache;
    private readonly MemoryCacheService _memoryCache;

    public HybridCacheService(
        IConnectionMultiplexer redis,
        IMemoryCache memoryCache)
    {
        _redisCache = new RedisCacheService(redis);
        _memoryCache = new MemoryCacheService(memoryCache);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime)
    {
        // Try to get from memory cache first (fastest)
        return await _memoryCache.GetOrSetAsync(key, async () =>
        {
            // If not in memory, try Redis
            return await _redisCache.GetOrSetAsync(key, getItemCallback, expirationTime);
        }, expirationTime);
    }

    public async Task RemoveAsync(string key)
    {
        await _redisCache.RemoveAsync(key);
        await _memoryCache.RemoveAsync(key);
    }
}