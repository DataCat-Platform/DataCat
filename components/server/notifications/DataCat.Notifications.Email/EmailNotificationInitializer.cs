namespace DataCat.Notifications.Email;

public sealed class EmailNotificationInitializer(IServiceProvider serviceProvider, ILogger<EmailNotificationInitializer> logger)
    : BaseNotificationInitializer(serviceProvider, logger)
{
    protected override async Task ExecuteAsync(IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync(cancellationToken);
        
        var notificationDestinationRepository = scopedProvider.GetRequiredService<INotificationDestinationRepository>();
        
        var existing =
            await notificationDestinationRepository.GetByNameAsync(EmailConstants.Email, cancellationToken);
        if (existing is null)
        {
            var telegramDestination = NotificationDestination.Create(EmailConstants.Email).Value;
            var id = await notificationDestinationRepository.AddAsync(telegramDestination, cancellationToken);
            
            logger.LogInformation("[{Job}] Initialized {NotificationDestinationName} notification destination (Id={DestinationId})", 
                nameof(EmailNotificationInitializer), EmailConstants.Email, id);
        }
        
        await unitOfWork.CommitAsync(cancellationToken);
    }
}