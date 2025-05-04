namespace DataCat.Server.Application.Queries.Namespaces.GetByName;

public sealed class GetNamespaceByNameQueryHandler(
    INamespaceRepository namespaceRepository,
    IDashboardRepository dashboardRepository)
    : IQueryHandler<GetNamespaceByNameQuery, NamespaceByNameResponse>
{
    public async Task<Result<NamespaceByNameResponse>> Handle(GetNamespaceByNameQuery request, CancellationToken cancellationToken)
    {
        var @namespace = await namespaceRepository.GetByNameAsync(request.Name, cancellationToken);
        if (@namespace is null)
        {
            return Result.Fail<NamespaceByNameResponse>(NamespaceError.NotFound(request.Name));
        }
        
        var dashboardsByNamespace = await dashboardRepository.GetDashboardsByNamespaceId(@namespace.Id, cancellationToken);

        var response = new NamespaceByNameResponse(@namespace.Name, dashboardsByNamespace.ToList());
        return Result.Success(response);
    }
}