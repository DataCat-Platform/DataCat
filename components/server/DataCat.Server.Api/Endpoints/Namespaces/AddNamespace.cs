namespace DataCat.Server.Api.Endpoints.Namespaces;

public sealed record AddNamespaceRequest(string Name);

// public sealed class AddNamespace : ApiEndpointBase
// {
//     public override void MapEndpoint(IEndpointRouteBuilder app)
//     {
//         app.MapPost("api/v{version:apiVersion}/namespace", async (
//                 [FromBody] AddNamespaceRequest request,
//                 [FromServices] IMediator mediator,
//                 CancellationToken token = default) =>
//             {
//                 var query = ToCommand(request);
//                 var result = await mediator.Send(query, token);
//                 return HandleCustomResponse(result);
//             })
//             .WithTags(ApiTags.Dashboards)
//             .HasApiVersion(ApiVersions.V1)
//             .Produces(StatusCodes.Status200OK)
//             .ProducesProblem(StatusCodes.Status400BadRequest);
//     }
//
//     private static AddUserToDashboardCommand ToCommand(AddNamespaceRequest request)
//     {
//         return new AddUserToDashboardCommand
//         {
//             UserId = request.UserId,
//             DashboardId = request.DashboardId,
//         };
//     } 
// }