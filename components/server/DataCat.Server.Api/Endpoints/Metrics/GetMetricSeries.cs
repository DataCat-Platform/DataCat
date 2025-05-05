namespace DataCat.Server.Api.Endpoints.Metrics;

// public sealed record GetMetricSeriesRequest(
//     string DataSourceName,
//     string MatchExpression,
//     int? Limit = null);
//
// public class GetMetricSeries : ApiEndpointBase
// {
//     public override void MapEndpoint(IEndpointRouteBuilder app)
//     {
//         app.MapPost("series", async (
//                 [FromServices] IMediator mediator,
//                 [FromBody] GetMetricSeriesRequest request,
//                 CancellationToken token = default) =>
//             {
//                 var command = new ListMetricSeriesCommand(
//                     request.DataSourceName,
//                     request.MatchExpression,
//                     request.Limit);
//                 
//                 var result = await mediator.Send(command, token);
//                 return HandleCustomResponse(result);
//             })
//             .WithTags(ApiTags.Metrics)
//             .HasApiVersion(ApiVersions.V1)
//             .Produces<IEnumerable<MetricSeries>>()
//             .ProducesProblem(StatusCodes.Status400BadRequest);
//     }
// }