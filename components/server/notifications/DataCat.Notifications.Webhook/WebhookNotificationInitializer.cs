namespace DataCat.Notifications.Webhook;

public sealed class WebhookNotificationInitializer(IServiceProvider serviceProvider, ILogger<WebhookNotificationInitializer> logger)
    : BaseNotificationInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var notificationDestinationRepository = scopedProvider.GetRequiredService<INotificationDestinationRepository>();
        
        var existing =
            await notificationDestinationRepository.GetByNameAsync(WebhookConstants.Webhook, cancellationToken);
        if (existing is null)
        {
            var telegramDestination = NotificationDestination.Create(WebhookConstants.Webhook).Value;
            var id = await notificationDestinationRepository.AddAsync(telegramDestination, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} notification destination (Id={DestinationId})", 
                nameof(WebhookNotificationInitializer), WebhookConstants.Webhook, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}