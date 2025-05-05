namespace DataCat.Server.Application.Scheduling;

public abstract class BaseBackgroundWorker(
    ILogger<BaseBackgroundWorker> logger,
    IMetricsContainer metricsContainer)
    : IJob
{
    private readonly Stopwatch StopWatch = new();
    protected abstract string JobName { get; }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var stopWatch = Stopwatch.StartNew();
        try
        {
            TrackJobStart();
            await RunAsync(context.CancellationToken);
        }
        catch (OperationCanceledException) when (!context.CancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("[{Job}] Job was cancelled", JobName);
        }
        finally
        {
            TrackJobEnd();
            stopWatch.Stop();
            metricsContainer.AddBackgroundJobExecution(JobName, stopWatch.ElapsedMilliseconds);
        }
    }

    private void TrackJobStart()
    {
        logger.LogInformation("[{Job}] Job started", JobName);
        StopWatch.Start();
    }

    private void TrackJobEnd()
    {
        StopWatch.Stop();
        logger.LogInformation("[{Job}] Job finished. Duration: {Duration} ms", JobName, StopWatch.ElapsedMilliseconds);
    }

    protected abstract Task RunAsync(CancellationToken stoppingToken = default);
}