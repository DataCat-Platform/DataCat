namespace DataCat.Server.Application.Security;

public interface ISecretsProvider
{
    Task<string> GetSecretAsync(string key, CancellationToken cancellationToken = default);
    Task SetSecretAsync(string key, string value, CancellationToken cancellationToken = default);
    Task DeleteSecretAsync(string key, CancellationToken cancellationToken = default);
    bool CanWrite { get; }
}