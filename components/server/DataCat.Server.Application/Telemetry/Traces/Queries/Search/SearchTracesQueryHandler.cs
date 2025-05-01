namespace DataCat.Server.Application.Telemetry.Traces.Queries.Search;

public sealed class SearchTracesQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager) 
    : IRequestHandler<SearchTracesQuery, Result<IEnumerable<TraceEntry>>>
{
    public async Task<Result<IEnumerable<TraceEntry>>> Handle(
        SearchTracesQuery request, 
        CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<TraceEntry>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var client = dataSourceManager.GetTracesClient(dataSource.Name);
        if (client is null)
            return Result.Fail<IEnumerable<TraceEntry>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await client.FindTracesAsync(
            request.Service,
            request.Start,
            request.End,
            request.Operation,
            request.Limit,
            request.MinDuration,
            request.MaxDuration,
            request.Tags,
            cancellationToken);

        return Result.Success(result);
    }
}