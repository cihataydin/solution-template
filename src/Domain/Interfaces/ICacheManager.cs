using System;

namespace Domain.Interfaces
{
    public interface ICacheManager
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);
        Task RemoveAsync(string key);
    }
}
