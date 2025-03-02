namespace DataCat.Secrets.Vault;

public sealed class VaultSecretConnectionOptions : ISecretConnectionOptions
{
    public required string ConnectionString { get; init; }
    
    public required string Login { get; init; }
    
    public required string Password { get; init; }
}