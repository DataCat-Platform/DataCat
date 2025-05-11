namespace DataCat.Server.Api.Endpoints.Users;

public sealed class GetMe : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/user/get-me", async (
                [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(new GetMeQuery());
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Users)
            .HasApiVersion(ApiVersions.V1)
            .Produces<GetMeResponse>()
            .WithCustomProblemDetails();
    }
}