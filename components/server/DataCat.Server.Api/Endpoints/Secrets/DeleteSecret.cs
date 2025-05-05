namespace DataCat.Server.Api.Endpoints.Secrets;

public sealed class DeleteSecret : ApiEndpointBase
{
    public override void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v{version:apiVersion}/secret/remove/{key}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string key,
                [FromServices] ISecretsProvider secretsProvider,
                CancellationToken token = default) =>
            {
                if (!secretsProvider.CanWrite)
                {
                    return Results.BadRequest("This secrets provider is read-only.");
                }
            
                await secretsProvider.DeleteSecretAsync(key, token);
                return Results.Ok("Secret removed");
            })
            .RequireAuthorization(opt =>
            {
                opt.RequireRole(UserRole.Admin.Name);
            })
            .WithTags(ApiTags.Secrets)
            .HasApiVersion(ApiVersions.V1)
            .Produces<string>()
            .Produces<string>(StatusCodes.Status400BadRequest);
    }
}