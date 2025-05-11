namespace DataCat.Server.Application.Factories;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    ValueTask<TConnection> GetOrCreateConnectionAsync(CancellationToken token);
}