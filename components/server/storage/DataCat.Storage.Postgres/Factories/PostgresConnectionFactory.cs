namespace DataCat.Storage.Postgres.Factories;

public class PostgresConnectionFactory(DatabaseOptions DbOptions)
    : IDbConnectionFactory<NpgsqlConnection>, IDisposable
{
    private NpgsqlConnection? _connection;
    private bool _isDisposed;

    public async ValueTask<NpgsqlConnection> GetOrCreateConnectionAsync(CancellationToken token)
    {
        if (_connection != null)
        {
            return _connection;
        }

        _connection = new NpgsqlConnection(DbOptions.ConnectionString);
        await _connection.OpenAsync(token);
        return _connection;
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
        _isDisposed = true;
    }
}