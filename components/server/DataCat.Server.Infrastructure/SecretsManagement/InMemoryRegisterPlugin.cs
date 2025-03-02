namespace DataCat.Server.Infrastructure.SecretsManagement;

public sealed class InMemoryRegisterPlugin : ISecretsPlugin
{
    public IServiceCollection RegisterSecretsStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISecretConnectionOptions, InMemorySecretConnectionOptions>(
            _ => new InMemorySecretConnectionOptions());
        services.AddSingleton<ISecretsProvider, InMemorySecretsProvider>();
        
        return services;
    }
}