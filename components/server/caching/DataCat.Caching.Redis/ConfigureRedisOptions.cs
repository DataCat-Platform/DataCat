namespace DataCat.Caching.Redis;

public sealed class ConfigureRedisOptions(
    IOptions<RedisSettings> settings,
    ISecretsProvider secretsProvider)
    : IConfigureOptions<RedisCacheOptions>
{
    public void Configure(RedisCacheOptions options)
    {
        var connectionString = GetConnectionString();
        var configOptions = ParseConnectionString(connectionString);

        options.ConfigurationOptions = configOptions;
        options.InstanceName = settings.Value.InstanceName;
    }

    private string GetConnectionString()
    {
        return secretsProvider.GetSecretAsync(settings.Value.ServerUrl!).GetAwaiter().GetResult();
    }

    private ConfigurationOptions ParseConnectionString(string connectionString)
    {
        var configOptions = new ConfigurationOptions
        {
            ConnectTimeout = settings.Value.ConnectTimeout,
            AbortOnConnectFail = settings.Value.AbortOnConnectFail,
        };

        var parts = connectionString.Split(',');
        configOptions.EndPoints.Add(parts[0]);

        var passwordPart = parts.FirstOrDefault(p => p.StartsWith("password="));
        if (!string.IsNullOrEmpty(passwordPart))
        {
            configOptions.Password = passwordPart.Split('=')[1];
        }

        return configOptions;
    }
}