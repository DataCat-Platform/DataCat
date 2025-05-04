namespace DataCat.Storage.Postgres.Repositories;

public sealed class NamespaceCachedRepository(
    ICacheService cache,
    NamespaceRepository namespaceRepository,
    IMetricsContainer metricsContainer)
    : IRepository<Namespace, Guid>, INamespaceRepository
{
    private const string DefaultNamespaceCacheKey = "namespace:default";
    private const string ByIdCacheKeyPrefix = "namespace:id:";
    private const string ByNameCacheKeyPrefix = "namespace:name:";
    
    public async Task<Namespace?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var cacheKey = $"{ByIdCacheKeyPrefix}{id}";
        
        var @namespace = await cache.GetOrCreateAsync(cacheKey, 
            factory: () => namespaceRepository.GetByIdAsync(id, token), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, token);
        
        return @namespace;
    }
    
    public async Task<Namespace?> GetByNameAsync(string name, CancellationToken token)
    {
        var cacheKey = $"{ByNameCacheKeyPrefix}{name}";
        
        var @namespace = await cache.GetOrCreateAsync(cacheKey, 
            factory: () => namespaceRepository.GetByNameAsync(name, token), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, token);
        
        return @namespace;
    }

    public async Task<Namespace> GetDefaultNamespaceAsync(CancellationToken token)
    {
        var @namespace = await cache.GetOrCreateAsync(DefaultNamespaceCacheKey, 
            factory: () => namespaceRepository.GetDefaultNamespaceAsync(token), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, token);
        
        return @namespace;
    }

    public Task AddAsync(Namespace entity, CancellationToken token = default)
    {
        return namespaceRepository.AddAsync(entity, token);
    }
}