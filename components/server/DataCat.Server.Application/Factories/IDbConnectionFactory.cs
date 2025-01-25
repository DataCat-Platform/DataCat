namespace DataCat.Server.Application.Factories;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    ValueTask<TConnection> CreateConnectionAsync(CancellationToken token);
}