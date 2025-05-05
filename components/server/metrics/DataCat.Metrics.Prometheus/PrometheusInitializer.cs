namespace DataCat.Metrics.Prometheus;

public sealed class PrometheusInitializer(IServiceProvider serviceProvider, ILogger<PrometheusInitializer> logger)
    : BaseDataSourceInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var dataSourceTypeRepository = scopedProvider.GetRequiredService<IDataSourceTypeRepository>();
        
        var existing =
            await dataSourceTypeRepository.GetByNameAsync(PrometheusConstants.Prometheus, cancellationToken);
        
        if (existing is null)
        {
            var dataSourceType = DataSourceType.Create(PrometheusConstants.Prometheus).Value;
            var id = await dataSourceTypeRepository.AddAsync(dataSourceType, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} metrics data source (Id={DataSourceTypeId})", 
                nameof(PrometheusInitializer), PrometheusConstants.Prometheus, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}