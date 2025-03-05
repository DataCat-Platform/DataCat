namespace DataCat.Server.Application.Queries.Alert.Search;

public class SearchAlertsQueryHandler(
    IDefaultRepository<AlertEntity, Guid> alertRepository)
    : IRequestHandler<SearchAlertsQuery, Result<List<AlertEntity>>>
{
    public async Task<Result<List<AlertEntity>>> Handle(SearchAlertsQuery request, CancellationToken token)
    {
        var result = await alertRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token)
            .ToListAsync(cancellationToken: token);
        return Result.Success(result);
    }
}