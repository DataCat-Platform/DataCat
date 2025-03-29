namespace DataCat.Server.Application.Persistence.Repositories;

public interface IAlertRepository
{
    Task<Page<AlertEntity>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);

    Task UpdateAsync(AlertEntity entity, CancellationToken token = default);
    
    Task DeleteAsync(Guid id, CancellationToken token = default);
}