namespace DataCat.Server.Application.Persistence.Repositories;

public interface INamespaceRepository
{
    Task<List<Namespace>> GetNamespacesForUserAsync(string identityId, CancellationToken token = default);
    
    Task<Namespace?> GetByNameAsync(string name, CancellationToken token);
    
    Task<Namespace> GetDefaultNamespaceAsync(CancellationToken token);
}