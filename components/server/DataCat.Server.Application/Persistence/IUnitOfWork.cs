namespace DataCat.Server.Application.Persistence;

public interface IUnitOfWork<T>
{
    T Transaction { get; }    
    ValueTask<T> StartTransactionAsync(CancellationToken cancellationToken = default);
    void SetIsolationLevel(DataCatDbIsolationLevel isolationLevel);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}