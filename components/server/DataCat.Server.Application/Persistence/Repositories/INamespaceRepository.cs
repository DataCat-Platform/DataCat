namespace DataCat.Server.Application.Persistence.Repositories;

public interface INamespaceRepository
{
    ValueTask<NamespaceEntity?> GetByNameAsync(string name, CancellationToken token);
    
    ValueTask<NamespaceEntity?> GetDefaultNamespaceAsync(CancellationToken token);
}