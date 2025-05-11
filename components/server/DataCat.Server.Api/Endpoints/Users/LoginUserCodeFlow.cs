namespace DataCat.Server.Api.Endpoints.Users;

public sealed class LoginUserCodeFlow : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/user/login-code-flow", (
                [FromServices] IOidcRedirectService oidcRedirectService) =>
            {
                var authUrl = oidcRedirectService.GenerateRedirectUrl();
                return Results.Redirect(authUrl);
            })
            .WithTags(ApiTags.Users)
            .HasApiVersion(ApiVersions.V1)
            .Produces(StatusCodes.Status302Found)
            .WithCustomProblemDetails();
    }
}