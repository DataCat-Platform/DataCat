namespace DataCat.Server.Application.Telemetry.Metrics.Queries.SearchRangeQuery;

public class SearchMetricsRangeQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager,
    IVariableService variableService) : IRequestHandler<SearchMetricsRangeQuery, Result<IEnumerable<TimeSeries>>>
{
    public async Task<Result<IEnumerable<TimeSeries>>> Handle(SearchMetricsRangeQuery request, CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<TimeSeries>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var searchClient = dataSourceManager.GetMetricsClient(dataSource.Name);
        if (searchClient is null)
            return Result.Fail<IEnumerable<TimeSeries>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var queryWithoutPlaceholders = await variableService.ResolveQueryVariablesAsync(
            request.Query,
            request.NamespaceId,
            request.DashboardId,
            cancellationToken); 
        
        var result = await searchClient.RangeQueryAsync(queryWithoutPlaceholders,
            request.Start,
            request.End,
            request.Step,
            cancellationToken);

        return Result.Success(result);
    }
    
}