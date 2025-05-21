namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchQuery;

public sealed class SearchMetricsQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager,
    NamespaceContext namespaceContext,
    IVariableService variableService) : IRequestHandler<SearchMetricsQuery, Result<IEnumerable<MetricPoint>>>
{
    public async Task<Result<IEnumerable<MetricPoint>>> Handle(SearchMetricsQuery request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<MetricPoint>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var searchClient = dataSourceManager.GetMetricsClient(dataSource.Name);
        if (searchClient is null)
            return Result.Fail<IEnumerable<MetricPoint>>(DataSourceError.NotFoundByName(request.DataSourceName));

        var queryWithoutPlaceholders = await variableService.ResolveQueryVariablesAsync(
            request.Query,
            namespaceId: Guid.Parse(namespaceContext.NamespaceId),
            request.DashboardId,
            cancellationToken); 
        
        var result = await searchClient.QueryAsync(queryWithoutPlaceholders, cancellationToken);

        return Result.Success(result);
    }
}