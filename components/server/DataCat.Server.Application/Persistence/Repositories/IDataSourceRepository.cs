namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDataSourceRepository
{
    Task<DataSource?> GetByNameAsync(string name, CancellationToken token = default);
    
    Task<IReadOnlyCollection<DataSource>> GetAllAsync(CancellationToken token = default);
    
    Task<Page<DataSource>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(DataSource entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}