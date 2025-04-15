using CachingAndDynamicUI.Models;

namespace CachingAndDynamicUI.Interfaces
{
    public interface IApiService
    {
        Task<List<User>> GetUsersAsync();
    }
}
