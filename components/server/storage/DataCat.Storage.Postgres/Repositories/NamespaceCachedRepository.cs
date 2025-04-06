namespace DataCat.Storage.Postgres.Repositories;

public sealed class NamespaceCachedRepository(
    IMemoryCache cache,
    NamespaceRepository namespaceRepository) 
    : IRepository<NamespaceEntity, Guid>, INamespaceRepository
{
    private const string DefaultNamespaceCacheKey = "namespace:default";
    private const string ByIdCacheKeyPrefix = "namespace:id:";
    private const string ByNameCacheKeyPrefix = "namespace:name:";
    
    public async Task<NamespaceEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var cacheKey = $"{ByIdCacheKeyPrefix}{id}";
        
        if (cache.TryGetValue(cacheKey, out NamespaceEntity? cached))
        {
            return cached!;
        }

        var entity = await namespaceRepository.GetByIdAsync(id, token);

        cache.Set(cacheKey, entity, TimeSpan.FromHours(1));
        return entity;
    }
    
    public async ValueTask<NamespaceEntity?> GetByNameAsync(string name, CancellationToken token)
    {
        var cacheKey = $"{ByNameCacheKeyPrefix}{name}";
        
        if (cache.TryGetValue(cacheKey, out NamespaceEntity? cached))
        {
            return cached!;
        }

        var entity = await namespaceRepository.GetByNameAsync(name, token);

        cache.Set(cacheKey, entity, TimeSpan.FromHours(1));
        return entity;
    }

    public async ValueTask<NamespaceEntity> GetDefaultNamespaceAsync(CancellationToken token)
    {
        if (cache.TryGetValue(DefaultNamespaceCacheKey, out NamespaceEntity? cached))
        {
            return cached!;
        }

        var entity = await namespaceRepository.GetDefaultNamespaceAsync(token);

        cache.Set(DefaultNamespaceCacheKey, entity, TimeSpan.FromHours(1));
        return entity;
    }

    public Task AddAsync(NamespaceEntity entity, CancellationToken token = default)
    {
        return namespaceRepository.AddAsync(entity, token);
    }
}