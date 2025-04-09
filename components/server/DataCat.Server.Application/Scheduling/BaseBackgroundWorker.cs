namespace DataCat.Server.Application.Scheduling;

public abstract class BaseBackgroundWorker(
    ILogger<BaseBackgroundWorker> logger)
    : IJob
{
    private readonly Stopwatch StopWatch = new();
    protected virtual string JobName => nameof(BaseBackgroundWorker);
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            TrackJobStart();
            await RunAsync(context.CancellationToken);
        }
        catch (OperationCanceledException) when (!context.CancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("[{Job}] Job was cancelled", GetType().Name);
        }
        finally
        {
            TrackJobEnd();
        }
    }

    protected void TrackJobStart()
    {
        logger.LogInformation("[{Job}] Job started", JobName);
        StopWatch.Start();
    }
    
    protected void TrackJobEnd()
    {
        StopWatch.Stop();
        logger.LogInformation("[{Job}] Job finished. Duration: {Duration} ms", JobName, StopWatch.ElapsedMilliseconds);
    }

    protected abstract Task RunAsync(CancellationToken stoppingToken = default);
}