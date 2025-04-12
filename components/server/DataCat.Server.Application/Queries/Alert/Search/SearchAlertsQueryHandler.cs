namespace DataCat.Server.Application.Queries.Alert.Search;

public sealed class SearchAlertsQueryHandler(
    IAlertRepository alertRepository)
    : IRequestHandler<SearchAlertsQuery, Result<Page<SearchAlertsResponse>>>
{
    public async Task<Result<Page<SearchAlertsResponse>>> Handle(SearchAlertsQuery request, CancellationToken token)
    {
        var result = await alertRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token);
        
        return Result.Success(result.ToResponsePage(SearchAlertsResponse.ToResponse));
    }
}