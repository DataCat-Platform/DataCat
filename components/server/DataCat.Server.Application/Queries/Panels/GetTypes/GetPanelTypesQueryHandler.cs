namespace DataCat.Server.Application.Queries.Panels.GetTypes;

public sealed class GetPanelTypesQueryHandler
    : IQueryHandler<GetPanelTypesQuery, List<GetPanelTypesResponse>>
{
    public Task<Result<List<GetPanelTypesResponse>>> Handle(GetPanelTypesQuery request, CancellationToken cancellationToken)
    {
        var result = PanelType.List.Select(x => new GetPanelTypesResponse(x.Value, x.Name)).ToList();
        return Task.FromResult(Result.Success(result));
    }
}