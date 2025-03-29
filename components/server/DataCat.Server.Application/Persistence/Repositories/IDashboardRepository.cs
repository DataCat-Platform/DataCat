namespace DataCat.Server.Application.Persistence.Repositories;

public interface IDashboardRepository
{
    Task AddUserToDashboard(UserEntity user, DashboardEntity dashboard, CancellationToken token = default);
    
    Task<Page<DashboardEntity>> SearchAsync(
        string? filter = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken token = default);
    
    Task UpdateAsync(DashboardEntity entity, CancellationToken token = default);

    Task DeleteAsync(Guid id, CancellationToken token = default);
}