using System.Text.Json;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infra.Cache;

public class CacheManagerOptions
{
    public TimeSpan MemoryCacheDuration { get; set; } = TimeSpan.FromMinutes(2);
    public TimeSpan DistributedCacheDuration { get; set; } = TimeSpan.FromMinutes(10);
    public JsonSerializerOptions? JsonOptions { get; set; } 
}

public class CacheManager: ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly CacheManagerOptions _options;

    public CacheManager(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        IOptions<CacheManagerOptions> options)
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _options = options.Value;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (_memoryCache.TryGetValue(key, out T? value))
            return value;

        var serialized = await _distributedCache.GetStringAsync(key);
        if (serialized is not null)
        {
            value = JsonSerializer.Deserialize<T>(serialized, _options.JsonOptions)!;
            _memoryCache.Set(key, value, _options.MemoryCacheDuration);
            return value;
        }

        return default;
    }

    public async Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        await _distributedCache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value)
    {
        this.SetMemory(key, value);
        await this.SetDistributedAsync(key, value);
    }
    
    private void SetMemory<T>(string key, T value, TimeSpan? duration = null)
    {
        _memoryCache.Set(key, value, duration ?? _options.MemoryCacheDuration);
    }

    private Task SetDistributedAsync<T>(string key, T value, TimeSpan? duration = null)
    {
        var serialized = JsonSerializer.Serialize(value, _options.JsonOptions);
        return _distributedCache.SetStringAsync(
            key,
            serialized,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration ?? _options.DistributedCacheDuration
            });
    }
}
