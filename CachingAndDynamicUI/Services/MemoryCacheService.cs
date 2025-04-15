using CachingAndDynamicUI.Interfaces;
using Microsoft.Extensions.Caching.Memory;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime)
    {
        if (_memoryCache.TryGetValue(key, out T cachedItem))
        {
            return cachedItem;
        }

        var item = await getItemCallback();

        if (item != null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expirationTime);

            _memoryCache.Set(key, item, cacheEntryOptions);
        }

        return item;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}