using Asp.Versioning;
using Asp.Versioning.Builder;

namespace DataCat.Server.Api.Endpoints.Secrets;

public sealed class AddSecret : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v{version:apiVersion}/secret/add", async (
                [FromServices] IMediator mediator,
                [FromBody] SecretModel secret,
                [FromServices] ISecretsProvider secretsProvider,
                CancellationToken token = default) =>
            {
                if (!secretsProvider.CanWrite)
                {
                    return Results.BadRequest("This secrets provider is read-only.");
                }
        
                await secretsProvider.SetSecretAsync(secret.Key, secret.Value, token);
                return Results.Ok("Secret added");
            })
            .WithTags(ApiTags.Secrets)
            .HasApiVersion(ApiVersions.V1)
            .Produces<string>()
            .Produces<string>(StatusCodes.Status400BadRequest);
    }
}