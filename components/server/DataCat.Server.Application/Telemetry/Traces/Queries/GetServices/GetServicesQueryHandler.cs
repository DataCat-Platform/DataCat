namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetServices;

public sealed class GetServicesQueryHandler(
    IDataSourceRepository dataSourceRepository,
    DataSourceManager dataSourceManager) 
    : IRequestHandler<GetServicesQuery, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(
        GetServicesQuery request, 
        CancellationToken cancellationToken)
    {
        var dataSource = await dataSourceRepository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<IEnumerable<string>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var client = dataSourceManager.GetTracesClient(dataSource.Name);
        if (client is null)
            return Result.Fail<IEnumerable<string>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await client.GetServicesAsync(cancellationToken);

        return Result.Success(result);
    }
}