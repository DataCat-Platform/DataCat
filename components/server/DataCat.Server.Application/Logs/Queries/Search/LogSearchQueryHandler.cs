namespace DataCat.Server.Application.Logs.Queries.Search;

public sealed class LogSearchQueryHandler(
    ISearchLogsClientFactory searchLogsClientFactory) : IRequestHandler<LogSearchQuery, Result<Page<LogEntry>>>
{
    public async Task<Result<Page<LogEntry>>> Handle(LogSearchQuery request, CancellationToken cancellationToken)
    {
        var searchClient = searchLogsClientFactory.CreateClient();
        var result = await searchClient.SearchAsync(request, cancellationToken);

        return Result.Success(result);
    }
}