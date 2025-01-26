namespace DataCat.Server.Postgres.Repositories;

public sealed class DashboardRepository(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IDefaultRepository<DashboardEntity, Guid>
{
    public Task<DashboardEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DashboardEntity>> SearchAsync(string? filter = null, int page = 1, int pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(DashboardEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(DashboardEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}