namespace DataCat.Notifications.Telegram;

public sealed class TelegramNotificationInitializer(IServiceProvider serviceProvider, ILogger<TelegramNotificationInitializer> logger)
    : BaseNotificationInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var notificationDestinationRepository = scopedProvider.GetRequiredService<INotificationDestinationRepository>();
        
        var existing =
            await notificationDestinationRepository.GetByNameAsync(TelegramConstants.Telegram, cancellationToken);
        if (existing is null)
        {
            var telegramDestination = NotificationDestination.Create(TelegramConstants.Telegram).Value;
            var id = await notificationDestinationRepository.AddAsync(telegramDestination, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} notification destination (Id={DestinationId})", 
                nameof(TelegramNotificationInitializer), TelegramConstants.Telegram, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}