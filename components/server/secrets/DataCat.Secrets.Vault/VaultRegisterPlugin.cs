namespace DataCat.Secrets.Vault;

public sealed class VaultRegisterPlugin : ISecretsPlugin
{
    public IServiceCollection RegisterSecretsStorage(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISecretsProvider, VaultSecretsProvider>();
        
        services.AddOptions<VaultSecretConnectionOptions>()
            .Bind(configuration.GetSection("Security"))
            .ValidateDataAnnotations()
            .Validate(options => 
            {
                if (options.AuthType == SecurityAuthType.Token)
                {
                    return !string.IsNullOrEmpty(options.TokenPath);
                }
                    
                return true;
            }, failureMessage: "TokenPath is required when AuthType is Token")
            .ValidateOnStart();
        
        services.AddSingleton<VaultSecretConnectionOptions>(sp => sp.GetRequiredService<IOptions<VaultSecretConnectionOptions>>().Value);
        
        return services;
    }
}