namespace DataCat.Secrets.Vault.Core;

internal sealed class VaultSecretsProvider
    : ISecretsProvider, IAsyncDisposable
{
    private readonly VaultSecretConnectionOptions _options;
    private readonly VaultClient _vaultClient;
    private readonly ConcurrentDictionary<string, (string Value, DateTime Expiry)> _secretCache = new();
    private readonly Timer _cacheCleanupTimer;
    
    public bool CanWrite => false;
    
    public VaultSecretsProvider(VaultSecretConnectionOptions options)
    {
        _options = options;
        _vaultClient = CreateVaultClient();
        _cacheCleanupTimer = new Timer(_ => CleanupCache(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }
    
    private VaultClient CreateVaultClient()
    {
        var settings = new VaultClientSettings(_options.ServerUri, GetAuthMethod())
        {
            Namespace = null,
        };

        return new VaultClient(settings);
    }
    
    private IAuthMethodInfo GetAuthMethod() => _options.AuthType switch
    {
        SecurityAuthType.Token => new TokenAuthMethodInfo(ResolveVaultToken()),
        SecurityAuthType.UserPass => new UserPassAuthMethodInfo(_options.Username, _options.Password),
        SecurityAuthType.Kerberos => throw new NotImplementedException("Kerberos auth not implemented"),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    private string ResolveVaultToken()
    {
        if (_options.AuthType != SecurityAuthType.Token || string.IsNullOrEmpty(_options.TokenPath))
        {
            throw new InvalidOperationException("Token auth requires TokenPath");
        }

        var envPath = Environment.GetEnvironmentVariable(_options.TokenPath);
        var actualPath = envPath ?? _options.TokenPath;

        if (!File.Exists(actualPath))
        {
            throw new FileNotFoundException("Vault token file not found", actualPath);
        }

        return File.ReadAllText(actualPath).Trim();
    }
    
    public async Task<string> GetSecretAsync(string key, CancellationToken cancellationToken = default)
    {
        if (TryGetCachedSecret(key, out var cachedValue))
        {
            return cachedValue!;
        }

        var (mountPoint, secretPath, secretKey) = ParseSecretKey(key);
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: secretPath, 
            mountPoint: mountPoint);

        if (!secret.Data.Data.TryGetValue(secretKey, out var value))
        {
            return string.Empty;
        }

        CacheSecret(key, value?.ToString() ?? string.Empty);
        return value?.ToString() ?? string.Empty;
    }

    public Task SetSecretAsync(string key, string value, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Vault secrets modification is not allowed");

    public Task DeleteSecretAsync(string key, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Vault secrets deletion is not allowed");
    
    private static (string MountPoint, string SecretPath, string SecretKey) ParseSecretKey(string key)
    {
        var parts = key.Split(["::"], StringSplitOptions.None);
        if (parts.Length != 3)
            throw new ArgumentException("Invalid key format. Expected format: 'mountPoint::secretPath::secretKey'");

        return (parts[0], parts[1], parts[2]);
    }
    
    private bool TryGetCachedSecret(string key, out string? value)
    {
        value = null;
        return _secretCache.TryGetValue(key, out var cached) && 
               DateTime.UtcNow < cached.Expiry && (value = cached.Value) != null;
    }
    
    private void CacheSecret(string key, string value)
    {
        var expiry = DateTime.UtcNow.Add(_options.CacheTtl);
        _secretCache[key] = (value, expiry);
    }
    
    private void CleanupCache()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in _secretCache)
        {
            if (entry.Value.Expiry < now)
            {
                _secretCache.TryRemove(entry.Key, out _);
            }
        }
    }
    
    public void Dispose()
    {
        _cacheCleanupTimer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _cacheCleanupTimer.DisposeAsync();
    }
}