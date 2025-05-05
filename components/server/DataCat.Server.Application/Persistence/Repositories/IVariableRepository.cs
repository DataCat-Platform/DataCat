namespace DataCat.Server.Application.Persistence.Repositories;

public interface IVariableRepository
{
    Task<List<Variable>> GetAllAsyncForDashboardAsync(Guid dashboardId, CancellationToken token = default);
    
    Task<List<Variable>> GetAllAsyncForNamespaceAsync(Guid namespaceId, CancellationToken token = default);
    
    Task UpdateAsync(Variable variable, CancellationToken token = default);
    
    Task DeleteAsync(Guid id, CancellationToken token = default);
}