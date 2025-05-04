namespace DataCat.Caching.Redis;

public sealed class RedisCacheService : ICacheService
{
    private const string GetOperation = "get";
    private const string SetOperation = "set";
    private const string RemoveOperation = "remove";
    
    private readonly IDistributedCache _cache;
    private readonly IMetricsContainer _metricsContainer;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
    };
    
    private readonly RedisAsyncLock _asyncLock = new();

    public RedisCacheService(IDistributedCache cache, IMetricsContainer metricsContainer)
    {
        _cache = cache;
        _metricsContainer = metricsContainer;

        _jsonOptions.AddAllJsonConverters(ApplicationAssembly.Assembly);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var cachedData = await _cache.GetStringAsync(key, ct);
            stopwatch.Stop();
            _metricsContainer.AddCacheOperationTime(GetOperation, stopwatch.ElapsedMilliseconds);

            if (cachedData is not null)
            {
                _metricsContainer.AddCacheHit(isHit: true, key);
                return JsonSerializer.Deserialize<T>(cachedData, _jsonOptions);
            }
            
            _metricsContainer.AddCacheHit(isHit: false, key);
            return default;
        }
        catch
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default)
    {
        var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
        var stopwatch = Stopwatch.StartNew();
        
        await _cache.SetStringAsync(key, serializedValue, options ?? new DistributedCacheEntryOptions(), ct);
        
        stopwatch.Stop();
        _metricsContainer.AddCacheOperationTime(
            operationType: SetOperation, 
            durationMs: stopwatch.ElapsedMilliseconds);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        var stopwatch = Stopwatch.StartNew();
        await _cache.RemoveAsync(key, ct);
        stopwatch.Stop();
        _metricsContainer.AddCacheOperationTime(
            operationType: SetOperation, 
            durationMs: stopwatch.ElapsedMilliseconds);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken ct = default)
    {
        var data = await _cache.GetStringAsync(key, ct);
        return data is not null;
    }

    public async Task RefreshAsync(string key, CancellationToken ct = default)
    {
        await _cache.RefreshAsync(key, ct);
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