namespace DataCat.Server.Api.Endpoints.Secrets;

public sealed class GetSecret : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
#if DEBUG
        app.MapGet("api/v{version:apiVersion}/secret/{key}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string key,
                [FromServices] ISecretsProvider secretsProvider,
                CancellationToken token = default) =>
            {
                var secret = await secretsProvider.GetSecretAsync(key, token);
                return Results.Ok(secret);
            })
            .RequireAuthorization(UserRole.Admin.Name)
            .WithTags(ApiTags.Secrets)
            .HasApiVersion(ApiVersions.V1)
            .Produces<string>();
#endif
    }
}