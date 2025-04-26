using DataCat.Server.Application.Commands.Users.Login;

namespace DataCat.Server.Api.Endpoints.Users;

public sealed record LoginUserRequest(
    string Email,
    string Password);

public sealed class LoginUser : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/user/login", async (
                [FromServices] IMediator mediator,
                [AsParameters] LoginUserRequest request,
                CancellationToken token = default) =>
            {
                var query = ToCommand(request);
                var result = await mediator.Send(query, token);
                return HandleCustomResponse(result);
            })
            .WithTags(ApiTags.Users)
            .HasApiVersion(ApiVersions.V1)
            .Produces<AccessTokenResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static LoginUserCommand ToCommand(LoginUserRequest request) 
        => new(request.Email, request.Password);
}