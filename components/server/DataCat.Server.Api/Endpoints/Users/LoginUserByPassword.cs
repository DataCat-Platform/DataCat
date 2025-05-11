namespace DataCat.Server.Api.Endpoints.Users;

public sealed record LoginUserByPasswordRequest(
    string Email,
    string Password);

public sealed class LoginUserByPassword : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/user/login", async (
                [FromServices] IMediator mediator,
                [AsParameters] LoginUserByPasswordRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Users)
            .HasApiVersion(ApiVersions.V1)
            .Produces<AccessTokenResponse>()
            .WithCustomProblemDetails();
    }

    private static LoginUserCommand ToCommand(LoginUserByPasswordRequest request) 
        => new(request.Email, request.Password);
}