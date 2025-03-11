namespace DataCat.Server.Infrastructure.SecretsManagement;

public sealed class InMemorySecretConnectionOptions : ISecretConnectionOptions
{
    public string ConnectionString => null!;
}