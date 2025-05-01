namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetTrace;

public sealed class GetTraceQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager) 
    : IRequestHandler<GetTraceQuery, Result<TraceEntry?>>
{
    public async Task<Result<TraceEntry?>> Handle(
        GetTraceQuery request, 
        CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<TraceEntry?>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var client = dataSourceManager.GetTracesClient(dataSource.Name);
        if (client is null)
            return Result.Fail<TraceEntry?>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await client.GetTraceAsync(request.TraceId, cancellationToken);

        return Result.Success(result);
    }
}