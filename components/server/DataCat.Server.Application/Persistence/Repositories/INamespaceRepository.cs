namespace DataCat.Server.Application.Persistence.Repositories;

public interface INamespaceRepository
{
    Task<Namespace?> GetByNameAsync(string name, CancellationToken token);
    
    Task<Namespace> GetDefaultNamespaceAsync(CancellationToken token);
}