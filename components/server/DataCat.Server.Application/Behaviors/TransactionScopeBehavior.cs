namespace DataCat.Server.Application.Behaviors;

public interface ITransactionScopeRequest;

public sealed class TransactionScopeBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionScopeRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var scope = new System.Transactions.TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            }, TransactionScopeAsyncFlowOption.Enabled);
        
        var result = await next();
        
        if (result.IsSuccess)
            scope.Complete();
        
        return result;
    }
}