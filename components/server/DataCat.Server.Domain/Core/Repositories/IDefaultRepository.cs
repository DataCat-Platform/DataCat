namespace DataCat.Server.Domain.Core.Repositories;

public interface IDefaultRepository<T, in TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken token);
        
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        
    Task AddAsync(T entity, CancellationToken token);
        
    Task UpdateAsync(T entity, CancellationToken token);
        
    Task DeleteAsync(TId id, CancellationToken token);
}