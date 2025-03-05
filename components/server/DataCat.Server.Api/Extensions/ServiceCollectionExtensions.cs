namespace DataCat.Server.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
    {
        services
            .AddSingleton<ExceptionHandlingMiddleware>()
            .AddSingleton<RequestLoggingMiddleware>()
            .AddTransient<AuthorizationMiddleware>();

        return services;
    }

    public static IServiceCollection AddApiBackgroundWorkers(this IServiceCollection services)
    {
        services.AddHostedService<AlertWorker>();
        return services;
    }
}