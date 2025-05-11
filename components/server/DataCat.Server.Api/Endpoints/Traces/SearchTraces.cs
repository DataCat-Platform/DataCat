namespace DataCat.Server.Api.Endpoints.Traces;

public sealed record SearchTracesRequest(
    string DataSourceName,
    string Service,
    DateTime Start,
    DateTime End,
    string? Operation = null,
    int? Limit = null,
    TimeSpan? MinDuration = null,
    TimeSpan? MaxDuration = null,
    Dictionary<string, object>? Tags = null);

public sealed class SearchTraces : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/traces/search", async (
                [FromServices] IMediator mediator,
                [FromBody] SearchTracesRequest request,
                CancellationToken token = default) =>
            {
                var query = ToQuery(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Traces)
            .HasApiVersion(ApiVersions.V1)
            .Produces<IEnumerable<TraceEntry>>()
            .WithCustomProblemDetails();
    }

    private static SearchTracesQuery ToQuery(SearchTracesRequest request)
    {
        return new SearchTracesQuery(
            request.DataSourceName,
            request.Service,
            request.Start,
            request.End,
            request.Operation,
            request.Limit,
            request.MinDuration,
            request.MaxDuration,
            request.Tags);
    }
}