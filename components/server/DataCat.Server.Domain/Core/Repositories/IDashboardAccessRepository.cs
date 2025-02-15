namespace DataCat.Server.Domain.Core.Repositories;

public interface IDashboardAccessRepository
{
    Task AddUserToDashboard(UserEntity user, DashboardEntity dashboard, CancellationToken token = default);
}