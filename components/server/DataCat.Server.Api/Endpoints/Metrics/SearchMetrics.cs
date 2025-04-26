// namespace DataCat.Server.Api.Endpoints.Metrics;
//
// public class SearchMetrics : ApiEndpointBase
// {
//     public override void MapEndpoint(IEndpointRouteBuilder app)
//     {
//         app.MapPost("api/v{version:apiVersion}/metrics/search", async (
//                 [FromServices] IMediator mediator,
//                 [FromBody] F request,
//                 CancellationToken token = default) =>
//             {
//                 // var query = ToQuery(request);
//                 // var result = await mediator.Send(query, token);
//                 // return HandleCustomResponse(result);
//                 return request;
//             })
//             .WithTags(ApiTags.Logs)
//             .HasApiVersion(ApiVersions.V1)
//             .Produces<Page<LogEntry>>()
//             .ProducesProblem(StatusCodes.Status400BadRequest);
//     }
// }
//
