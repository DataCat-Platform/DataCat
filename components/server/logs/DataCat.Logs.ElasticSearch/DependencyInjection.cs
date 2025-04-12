using DataCat.Logs.ElasticSearch.Searching;

namespace DataCat.Logs.ElasticSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddElasticSearchLogSearching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<ElasticSearchSettings>()
            .BindConfiguration(ElasticSearchSettings.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<ISearchLogsSettings>(sp =>
            sp.GetRequiredService<IOptions<ElasticSearchSettings>>().Value);

        services.AddScoped<ISearchLogsClientFactory, ElasticSearchClientFactory>();
        
        return services;
    }
}