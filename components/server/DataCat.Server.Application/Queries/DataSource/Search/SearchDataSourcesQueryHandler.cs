namespace DataCat.Server.Application.Queries.DataSource.Search;

public class SearchDataSourcesQueryHandler(
    IDataSourceRepository dataSourceRepository)
    : IRequestHandler<SearchDataSourcesQuery, Result<Page<SearchDataSourcesResponse>>>
{
    public async Task<Result<Page<SearchDataSourcesResponse>>> Handle(SearchDataSourcesQuery request, CancellationToken token)
    {
        var result = await dataSourceRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token);
        
        return Result.Success(result.ToResponsePage(SearchDataSourcesResponse.ToResponse));
    }
}