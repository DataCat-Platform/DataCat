namespace DataCat.Server.Application.Persistence.Repositories;

public interface INamespaceRepository
{
    ValueTask<Namespace?> GetByNameAsync(string name, CancellationToken token);
    
    ValueTask<Namespace> GetDefaultNamespaceAsync(CancellationToken token);
}