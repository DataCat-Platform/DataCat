namespace DataCat.Caching.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var enableDistributedCacheStr = configuration["EnableDistributedCache"];

        if (!bool.TryParse(enableDistributedCacheStr, out var enableDistributedCache) || !enableDistributedCache)
        {
            return services;
        }
        
        services.AddOptions<RedisSettings>()
            .Bind(configuration.GetSection(RedisSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.ConfigureOptions<ConfigureRedisOptionsSetup>();
        
        services.AddStackExchangeRedisCache(_ => {});

        services.AddSingleton<ICacheService, RedisCacheService>();
        
        return services;
    }
}