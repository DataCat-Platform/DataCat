namespace DataCat.Server.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
    {
        services
            .AddSingleton<ExceptionHandlingMiddleware>()
            .AddSingleton<RequestLoggingMiddleware>();

        return services;
    }
}