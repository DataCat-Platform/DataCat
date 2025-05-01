namespace DataCat.Secrets.Vault.Core;

internal sealed class VaultSecretConnectionOptions : ISecretConnectionOptions
{
    public required SecurityAuthType AuthType { get; init; }
    public required string ServerUri { get; init; }
    public string? TokenPath { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public TimeSpan CacheTtl { get; init; } = TimeSpan.FromMinutes(30);
}