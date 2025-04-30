namespace DataCat.Server.Api.Endpoints.Metrics;

// public sealed record GetLatestMetricRequest(
//     string DataSourceName,
//     string MetricName);
//     
// public class GetLatestMetric : ApiEndpointBase
// {
//     public override void MapEndpoint(IEndpointRouteBuilder app)
//     {
//         app.MapGet("latest", async (
//                 [FromServices] IMediator mediator,
//                 [FromQuery] string dataSourceName,
//                 [FromQuery] string metricName,
//                 CancellationToken token = default) =>
//             {
//                 var command = new GetLatestMetricCommand(dataSourceName, metricName);
//                 var result = await mediator.Send(command, token);
//                 return HandleCustomResponse(result);
//             })
//             .WithTags(ApiTags.Metrics)
//             .HasApiVersion(ApiVersions.V1)
//             .Produces<MetricPoint>()
//             .ProducesProblem(StatusCodes.Status400BadRequest);;
//     }
// }