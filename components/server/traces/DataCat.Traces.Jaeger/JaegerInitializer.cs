namespace DataCat.Traces.Jaeger;

public sealed class JaegerInitializer(IServiceProvider serviceProvider, ILogger<JaegerInitializer> logger)
    : BaseDataSourceInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var dataSourceTypeRepository = scopedProvider.GetRequiredService<IDataSourceTypeRepository>();
        
        var existing =
            await dataSourceTypeRepository.GetByNameAsync(JaegerConstants.Jaeger, cancellationToken);
        
        if (existing is null)
        {
            var dataSourceType = DataSourceType.Create(JaegerConstants.Jaeger).Value;
            var id = await dataSourceTypeRepository.AddAsync(dataSourceType, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} traces data source (Id={DataSourceTypeId})", 
                nameof(JaegerInitializer), JaegerConstants.Jaeger, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}