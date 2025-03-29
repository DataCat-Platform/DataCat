namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDataSourceRepository
{
    Task<Page<DataSourceEntity>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(DataSourceEntity entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}