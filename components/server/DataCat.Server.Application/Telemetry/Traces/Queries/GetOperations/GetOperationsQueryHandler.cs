namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetOperations;

public sealed class GetOperationsQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager) 
    : IRequestHandler<GetOperationsQuery, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(
        GetOperationsQuery request, 
        CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<string>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var client = dataSourceManager.GetTracesClient(dataSource.Name);
        if (client is null)
            return Result.Fail<IEnumerable<string>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await client.GetOperationsAsync(request.ServiceName, cancellationToken);

        return Result.Success(result);
    }
}