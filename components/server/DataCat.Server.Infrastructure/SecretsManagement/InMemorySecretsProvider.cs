namespace DataCat.Server.Infrastructure.SecretsManagement;

public class InMemorySecretsProvider : ISecretsProvider
{
    private readonly ConcurrentDictionary<string, string> _secrets = new();

    public Task<string> GetSecretAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_secrets.TryGetValue(key, out var value))
        {
            return Task.FromResult(value);
        }
        var envValue = Environment.GetEnvironmentVariable(key);
        
        return envValue is null 
            ? throw new KeyNotFoundException()
            : Task.FromResult(envValue);
    }

    public Task SetSecretAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        CheckWritable();
        _secrets[key] = value;
        return Task.CompletedTask;
    }

    public Task DeleteSecretAsync(string key, CancellationToken cancellationToken = default)
    {
        CheckWritable();
        _secrets.Remove(key, out _);
        return Task.CompletedTask;
    }

    public bool CanWrite => true;

    private void CheckWritable()
    {
        if (!CanWrite)
        {
            throw new InvalidOperationException("Secret cannot be written");
        }
    }
}