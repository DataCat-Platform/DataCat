namespace DataCat.Server.Application.Queries.DataSources.Search;

public class SearchDataSourcesQueryHandler(
    IDataSourceRepository dataSourceRepository)
    : IQueryHandler<SearchDataSourcesQuery, Page<SearchDataSourcesResponse>>
{
    public async Task<Result<Page<SearchDataSourcesResponse>>> Handle(SearchDataSourcesQuery request, CancellationToken token)
    {
        var result = await dataSourceRepository
            .SearchAsync(request.Filters, request.Page, request.PageSize, token);
        
        return Result.Success(result.ToResponsePage(SearchDataSourcesResponse.ToResponse));
    }
}