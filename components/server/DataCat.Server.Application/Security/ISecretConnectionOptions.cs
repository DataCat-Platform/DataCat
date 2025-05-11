namespace DataCat.Server.Application.Security;

public interface ISecretConnectionOptions
{
    SecurityAuthType AuthType { get; }
    string ServerUri { get; }
    string? TokenPath { get; }
    string? Username { get; }
    string? Password { get; }
    TimeSpan CacheTtl { get; }
}