namespace DataCat.Storage.Postgres.Factories;

public class PostgresConnectionFactory(IOptions<DatabaseOptions> DbOptions)
    : IDbConnectionFactory<NpgsqlConnection>
{
    private NpgsqlConnection? _connection;

    public async ValueTask<NpgsqlConnection> CreateConnectionAsync(CancellationToken token)
    {
        if (_connection != null) return _connection;

        _connection = new NpgsqlConnection(DbOptions.Value.ConnectionString);
        await _connection.OpenAsync(token);
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}