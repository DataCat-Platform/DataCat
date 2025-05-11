using DataCat.Server.Domain.Core;

namespace DataCat.Logs.ElasticSearch;

public sealed class ElasticSearchInitializer(IServiceProvider serviceProvider, ILogger<ElasticSearchInitializer> logger)
    : BaseDataSourceInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var dataSourceTypeRepository = scopedProvider.GetRequiredService<IDataSourceTypeRepository>();
        
        var existing =
            await dataSourceTypeRepository.GetByNameAsync(ElasticSearchConstants.ElasticSearch, cancellationToken);
        
        if (existing is null)
        {
            var dataSourceType = DataSourceType.Create(ElasticSearchConstants.ElasticSearch).Value;
            var id = await dataSourceTypeRepository.AddAsync(dataSourceType, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} logs data source (Id={DataSourceTypeId})", 
                nameof(ElasticSearchInitializer), ElasticSearchConstants.ElasticSearch, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}