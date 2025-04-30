namespace DataCat.Server.Application.Telemetry;

public sealed class DataSourceContainerLoaderJob(
    DataSourceContainer container, 
    IServiceProvider serviceProvider, 
    ILogger<DataSourceContainerLoaderJob> logger) : BaseBackgroundWorker(logger)
{
    protected override string JobName => nameof(DataSourceContainerLoaderJob);

    protected override async Task RunAsync(CancellationToken stoppingToken = default)
    {
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(stoppingToken);
        
        var dataSourceRepository = serviceProvider.GetRequiredService<IDataSourceRepository>();
        
        var dataSources =
            await dataSourceRepository.GetAllAsync(stoppingToken);

        var metrics = dataSources.Where(x => x.Purpose == DataSourcePurpose.Metrics);
        var logs = dataSources.Where(x => x.Purpose == DataSourcePurpose.Logs);
        var traces = dataSources.Where(x => x.Purpose == DataSourcePurpose.Traces);
        
        container.Load(DataSourceKind.Metrics, metrics);
        container.Load(DataSourceKind.Logs, logs);
        container.Load(DataSourceKind.Traces, traces);
        
        await unitOfWork.CommitAsync(stoppingToken);
    }
}