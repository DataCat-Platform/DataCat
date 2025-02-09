namespace DataCat.Server.Application.Queries.DataSource.Search;

public class SearchDataSourcesQueryHandler(
    IDefaultRepository<DataSourceEntity, Guid> dataSourceRepository)
    : IRequestHandler<SearchDataSourcesQuery, Result<List<DataSourceEntity>>>
{
    public async Task<Result<List<DataSourceEntity>>> Handle(SearchDataSourcesQuery request, CancellationToken token)
    {
        var result = await dataSourceRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token)
            .ToListAsync(cancellationToken: token);
        return Result.Success(result);
    }
}