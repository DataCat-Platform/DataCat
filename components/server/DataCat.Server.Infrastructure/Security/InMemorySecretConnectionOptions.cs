namespace DataCat.Server.Infrastructure.Security;

public sealed class InMemorySecretConnectionOptions : ISecretConnectionOptions
{
    public SecurityAuthType AuthType => SecurityAuthType.None;
    public string ServerUri => string.Empty;
    public string? TokenPath => string.Empty;
    public string? Username => string.Empty;
    public string? Password => string.Empty;
    public TimeSpan CacheTtl => TimeSpan.Zero;
}