namespace DataCat.Storage.Postgres.Persistence;

public interface ITransactionScopeRequest
{
    PostgresIsolationLevel GetIsolationLevel() => PostgresIsolationLevel.ReadCommitted;   
}

public sealed class TransactionScopeBehavior<TRequest, TResponse>(
    UnitOfWork UnitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        PostgresIsolationLevel? isolationLevel = null;
        
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (request is ITransactionScopeRequest transactionRequest)
        {
            isolationLevel = transactionRequest.GetIsolationLevel();
        }
        return await HandleTransactionAsync(next, cancellationToken, isolationLevel);
    }
    
    private async Task<TResponse> HandleTransactionAsync(
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken,
        PostgresIsolationLevel? isolationLevel = null)
    {
        if (isolationLevel.HasValue)
        {
            UnitOfWork.SetIsolationLevel(isolationLevel.Value);
        }

        await UnitOfWork.StartTransactionAsync(cancellationToken);
        var result = await next();
        
        if (result.IsSuccess)
        {
            await UnitOfWork.CommitAsync(cancellationToken);
        }
        else
        {
            await UnitOfWork.RollbackAsync(cancellationToken);
        }

        return result;
    }
}
