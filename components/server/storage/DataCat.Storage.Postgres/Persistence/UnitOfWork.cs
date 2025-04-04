namespace DataCat.Storage.Postgres.Persistence;

public sealed class UnitOfWork(
    IDbConnectionFactory<NpgsqlConnection> Factory)
    : IUnitOfWork<IDbTransaction>, IDisposable
{
    private NpgsqlTransaction? _transaction;
    private PostgresIsolationLevel _isolationLevel = PostgresIsolationLevel.ReadCommitted;
    private bool _disposed;

    public IDbTransaction Transaction => 
        _transaction 
        ?? throw new InvalidOperationException("Transaction has not been started");

    public async ValueTask<IDbTransaction> StartTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            return _transaction;
        }

        var connection = await Factory.GetOrCreateConnectionAsync(cancellationToken);
        _transaction = await connection.BeginTransactionAsync(_isolationLevel, cancellationToken);
        return _transaction;
    }

    public void SetIsolationLevel(PostgresIsolationLevel isolationLevel)
    {
        _isolationLevel = isolationLevel;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("The transaction has not been committed");
        }

        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("The transaction has not been rollback");
        }

        await _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            Factory.Dispose();    
        }
        _disposed = true;
    }
}
