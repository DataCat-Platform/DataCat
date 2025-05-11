using DataCat.Server.Application.Commands.Users.GetAccessTokenByCode;

namespace DataCat.Server.Api.Endpoints.Users;

public sealed class OidcCallbackEndpoint : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v{version:apiVersion}/user/callback", async (
                HttpContext httpContext,
                [FromQuery] string code,
                [FromServices] IMediator mediator,
                CancellationToken token = default) =>
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Results.BadRequest("Missing code");
                }

                var command = new GetAccessTokenByCodeCommand(code);
                var result = await mediator.Send(command, token);

                if (result.IsFailure)
                    return HandleCustomResponse(result);

                var accessToken = result.Value.AccessToken;

                httpContext.Response.Cookies.Append("access_token", accessToken, new CookieOptions
                {
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    MaxAge = TimeSpan.FromHours(1)
                });

                return Results.Redirect("/");
            })
            .WithTags(ApiTags.Users)
            .Produces(StatusCodes.Status302Found)
            .WithCustomProblemDetails();
    }
}