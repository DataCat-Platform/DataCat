namespace DataCat.Server.Domain.Core.Repositories;

public interface IDefaultRepository<T, in TId>
{
    Task<T?> GetByIdAsync(TId id);
        
    Task<IEnumerable<T>> GetAllAsync();
        
    Task AddAsync(T dataSource);
        
    Task UpdateAsync(T dataSource);
        
    Task DeleteAsync(TId id);
}