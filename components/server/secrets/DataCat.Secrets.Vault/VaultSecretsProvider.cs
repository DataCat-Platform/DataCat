namespace DataCat.Secrets.Vault;

public sealed class VaultSecretsProvider(
    VaultSecretConnectionOptions connectionOptions) 
    : ISecretsProvider
{
    public Task<string> GetSecretAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetSecretAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSecretAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public bool CanWrite => false;
}