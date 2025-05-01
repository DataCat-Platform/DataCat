namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchQuery;

public sealed class SearchMetricsQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager) : IRequestHandler<SearchMetricsQuery, Result<IEnumerable<MetricPoint>>>
{
    public async Task<Result<IEnumerable<MetricPoint>>> Handle(SearchMetricsQuery request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<MetricPoint>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var searchClient = dataSourceManager.GetMetricsClient(dataSource.Name);
        if (searchClient is null)
            return Result.Fail<IEnumerable<MetricPoint>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await searchClient.QueryAsync(request.Query, cancellationToken);

        return Result.Success(result);
    }
}