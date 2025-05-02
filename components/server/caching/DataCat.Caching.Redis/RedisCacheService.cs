namespace DataCat.Caching.Redis;

public sealed class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };
    
    private readonly RedisAsyncLock _asyncLock = new();

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        try
        {
            var cachedData = await cache.GetStringAsync(key, ct);
            return cachedData is null 
                ? default 
                : JsonSerializer.Deserialize<T>(cachedData, _jsonOptions);
        }
        catch
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default)
    {
        var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
        await cache.SetStringAsync(key, serializedValue, options ?? new DistributedCacheEntryOptions(), ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await cache.RemoveAsync(key, ct);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken ct = default)
    {
        var data = await cache.GetStringAsync(key, ct);
        return data is not null;
    }

    public async Task RefreshAsync(string key, CancellationToken ct = default)
    {
        await cache.RefreshAsync(key, ct);
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DistributedCacheEntryOptions? options = null,
        CancellationToken ct = default)
    {
        using (await _asyncLock.AcquireAsync(key))
        {
            var cachedValue = await GetAsync<T>(key, ct);
            if (cachedValue is not null)
                return cachedValue;

            var value = await factory();
            await SetAsync(key, value, options, ct);
            return value;
        }
    }
}