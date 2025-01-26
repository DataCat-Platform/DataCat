namespace DataCat.Server.Postgres.Repositories;

public class DataSourceRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<DataSource, Guid>
{
    public Task<DataSource?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DataSource>> SearchAsync(string? filter = null, int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(DataSource entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(DataSource entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}