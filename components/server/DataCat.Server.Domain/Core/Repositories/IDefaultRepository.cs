namespace DataCat.Server.Domain.Core.Repositories;

public interface IDefaultRepository<T, in TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken token = default);
        
    IAsyncEnumerable<T> SearchAsync(string? filter = null, int page = 1, int pageSize = 10, CancellationToken token = default);
        
    Task AddAsync(T entity, CancellationToken token = default);
        
    Task UpdateAsync(T entity, CancellationToken token = default);
        
    Task DeleteAsync(TId id, CancellationToken token = default);
}