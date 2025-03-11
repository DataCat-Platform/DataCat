namespace DataCat.Storage.Postgres.Jobs;

public abstract class BaseBackgroundWorker(
    ILogger<BaseBackgroundWorker> logger)
    : IJob
{
    private readonly Stopwatch StopWatch = new();
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await RunAsync(context.CancellationToken);
        }
        catch (OperationCanceledException) when (!context.CancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("[{Job}] Job was cancelled", GetType().Name);
        }
    }

    protected void TrackJobStart(string jobName)
    {
        logger.LogInformation("[{Job}] Job started", jobName);
        StopWatch.Start();
    }
    
    protected void TrackJobEnd(string jobName)
    {
        StopWatch.Stop();
        logger.LogInformation("[{Job}] Job finished. Duration: {Duration} ms", jobName, StopWatch.ElapsedMilliseconds);
    }

    protected abstract Task RunAsync(CancellationToken stoppingToken = default);
}