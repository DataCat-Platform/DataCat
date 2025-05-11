namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDataSourceTypeRepository
{
    Task<List<DataSourceType>> GetAllAsync(CancellationToken token = default);
    
    Task<DataSourceType?> GetByNameAsync(string name, CancellationToken token = default);
    
    Task<int> AddAsync(DataSourceType dataSourceType, CancellationToken token = default);
    
    Task DeleteAsync(int id, CancellationToken token = default);
}