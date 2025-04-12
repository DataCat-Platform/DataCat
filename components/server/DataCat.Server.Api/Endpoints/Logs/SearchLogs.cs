namespace DataCat.Server.Api.Endpoints.Logs;

public sealed record SearchLogsRequest(
    string? TraceId = null,
    DateTime? From = null,
    DateTime? To = null,
    string? Severity = null,
    string? ServiceName = null,
    Dictionary<string, string>? CustomFilters = null,
    int PageSize = 100,
    int Page = 1,
    string? SortField = null,
    bool SortAscending = false);

public sealed class SearchLogs : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/logs/search", async (
                [FromServices] IMediator mediator,
                [FromBody] SearchLogsRequest request,
                CancellationToken token = default) =>
            {
                var query = ToQuery(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Logs)
            .HasApiVersion(ApiVersions.V1)
            .Produces<Page<LogEntry>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
    
    private static LogSearchQuery ToQuery(SearchLogsRequest request)
    {
        return new LogSearchQuery(
            request.TraceId,
            request.From,
            request.To,
            request.Severity,
            request.ServiceName,
            request.CustomFilters,
            request.PageSize,
            request.Page,
            request.SortField,
            request.SortAscending
        );
    }

}