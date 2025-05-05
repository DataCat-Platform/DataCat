namespace DataCat.Logs.ElasticSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddElasticSearchLogSearching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ILogsClientFactory, ElasticClientFactory>();
        services.AddHostedService<ElasticSearchInitializer>();
        
        return services;
    }
}