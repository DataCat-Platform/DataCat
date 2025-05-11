namespace DataCat.Server.Application.Services;

public interface INamespaceService
{
    Task<Result<Namespace>> GetSpecificNamespaceOrDefaultAsync(string? namespaceId, CancellationToken token = default);
}

public class NamespaceService(
    IRepository<Namespace, Guid> defaultNamespaceRepository,
    INamespaceRepository namespaceRepository): INamespaceService
{
    public async Task<Result<Namespace>> GetSpecificNamespaceOrDefaultAsync(
        string? namespaceId, 
        CancellationToken token = default)
    {
        if (namespaceId is null)
        {
            return Result.Success(await namespaceRepository.GetDefaultNamespaceAsync(token));
        }

        if (!Guid.TryParse(namespaceId, out var namespaceIdGuid))
        {
            return Result.Fail<Namespace>("Namespace Id is not a Guid");
        }
        
        var @namespace = await defaultNamespaceRepository.GetByIdAsync(namespaceIdGuid, token);
        
        return @namespace is null 
            ? Result.Fail<Namespace>("Namespace not found") 
            : Result.Success(@namespace);
    }
}