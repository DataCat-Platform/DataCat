namespace DataCat.Server.Api.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static IEndpointConventionBuilder WithCustomProblemDetails(this RouteHandlerBuilder builder)
    {
        return builder
            .Produces<CustomProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<CustomProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<CustomProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}