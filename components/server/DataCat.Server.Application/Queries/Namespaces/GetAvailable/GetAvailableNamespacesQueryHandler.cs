namespace DataCat.Server.Application.Queries.Namespaces.GetAvailable;

public sealed class GetAvailableNamespacesQueryHandler(
    INamespaceRepository namespaceRepository,
    IIdentity identity) 
    : IQueryHandler<GetAvailableNamespacesQuery, List<GetAvailableNamespaceResponse>>
{
    public async Task<Result<List<GetAvailableNamespaceResponse>>> Handle(GetAvailableNamespacesQuery request, CancellationToken cancellationToken)
    {
        var namespaces = await namespaceRepository.GetNamespacesForUserAsync(identity.IdentityId, cancellationToken);
        return Result.Success(namespaces.Select(x => new GetAvailableNamespaceResponse(x.Id.ToString(), x.Name)).ToList());
    }
}