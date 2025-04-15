namespace CachingAndDynamicUI.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime);
        Task RemoveAsync(string key);
    }

}
