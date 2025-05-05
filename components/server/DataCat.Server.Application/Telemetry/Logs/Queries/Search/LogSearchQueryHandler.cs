namespace DataCat.Server.Application.Telemetry.Logs.Queries.Search;

public sealed class LogSearchQueryHandler(
    IDataSourceRepository repository,
    DataSourceManager dataSourceManager) : IRequestHandler<LogSearchQuery, Result<Page<LogEntry>>>
{
    public async Task<Result<Page<LogEntry>>> Handle(LogSearchQuery request, CancellationToken cancellationToken)
    {
        var dataSource = await repository.GetByNameAsync(request.DataSourceName, cancellationToken);
        if (dataSource is null)
            return Result.Fail<Page<LogEntry>>(DataSourceError.NotFoundByName(request.DataSourceName));

        using var searchClient = dataSourceManager.GetLogsClient(dataSource.Name);
        if (searchClient is null)
            return Result.Fail<Page<LogEntry>>(DataSourceError.NotFoundByName(request.DataSourceName));
        
        var result = await searchClient.SearchAsync(request, cancellationToken);

        return Result.Success(result);
    }
}