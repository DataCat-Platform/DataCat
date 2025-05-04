namespace DataCat.Server.Application.Behaviors;

public sealed class CommandMetricsBehavior<TRequest, TResponse>(IMetricsContainer metricsContainer)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await next();
        stopwatch.Stop();

        // todo: add caching for reflection
        var commandType = request.GetType().Name;
        var durationMs = stopwatch.ElapsedMilliseconds;
        var isSuccess = result.IsSuccess;
        
        metricsContainer.AddCommandExecution(commandType, durationMs, isSuccess);
        
        return result;
    }
}

public sealed class QueryMetricsBehavior<TRequest, TResponse>(IMetricsContainer metricsContainer)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await next();
        stopwatch.Stop();

        // todo: add caching for reflection
        var commandType = request.GetType().Name;
        var durationMs = stopwatch.ElapsedMilliseconds;
        var isSuccess = result.IsSuccess;
        
        metricsContainer.AddQueryExecution(commandType, durationMs, isSuccess);
        
        return result;
    }
}