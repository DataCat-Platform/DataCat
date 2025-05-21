namespace DataCat.Server.Api.Endpoints.Metrics;

public sealed record SearchMetricsRequest(
    string DataSourceName,
    string Query,
    Guid? DashboardId = null,
    DateTime? From = null,
    DateTime? To = null,
    TimeSpan? Step = null);

public class SearchMetrics : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/metrics")
            .WithTags(ApiTags.Metrics)
            .HasApiVersion(ApiVersions.V1);
        
        group.MapGet("query", async (
                [FromServices] IMediator mediator,
                [AsParameters] SearchMetricsRequest request,
                CancellationToken token = default) =>
            {
                var query = ToQuery(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .Produces<IEnumerable<MetricPoint>>()
            .WithCustomProblemDetails();
        
        group.MapGet("query-range", async (
                [FromServices] IMediator mediator,
                [AsParameters] SearchMetricsRequest request,
                CancellationToken token = default) =>
            {
                if (!request.From.HasValue || !request.To.HasValue || !request.Step.HasValue)
                {
                    return Results.BadRequest("From, To and Step parameters are required for range queries");
                }

                var query = ToRangeQuery(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .Produces<IEnumerable<TimeSeries>>()
            .WithCustomProblemDetails();
    }
    
    private static SearchMetricsQuery ToQuery(SearchMetricsRequest request)
    {
        return new SearchMetricsQuery(
            request.DataSourceName, 
            request.Query,
            request.DashboardId);
    }
    
    private static SearchMetricsRangeQuery ToRangeQuery(SearchMetricsRequest request)
    {
        return new SearchMetricsRangeQuery(
            request.DataSourceName, 
            request.Query,
            Start: request.From!.Value,
            End: request.To!.Value,
            Step: request.Step!.Value,
            DashboardId: request.DashboardId);
    }
}

