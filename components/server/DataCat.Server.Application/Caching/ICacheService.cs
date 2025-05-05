namespace DataCat.Server.Application.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default);
    
    Task RemoveAsync(string key, CancellationToken ct = default);
    
    Task<bool> ExistsAsync(string key, CancellationToken ct = default);
    
    Task RefreshAsync(string key, CancellationToken ct = default);
    
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, 
        DistributedCacheEntryOptions? options = null, CancellationToken ct = default);
}