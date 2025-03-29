namespace DataCat.Server.Domain.Common;

public interface IRepository<T, in TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken token = default);
        
    Task AddAsync(T entity, CancellationToken token = default);
}