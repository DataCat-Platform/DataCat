namespace DataCat.Server.Application.Commands.NotificationChannel.Add;

public sealed class AddNotificationCommandHandler(
    IDefaultRepository<NotificationChannelEntity, Guid> notificationChannelRepository)
    : IRequestHandler<AddNotificationCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        var destinationType = NotificationDestination.FromValue(request.DestinationType);
        if (destinationType is null)
            return Result.Fail<Guid>(NotificationChannelError.DestinationNotSupported);

        var notificationResult = NotificationChannelEntity.Create(
            Guid.NewGuid(),
            destinationType,
            request.Settings);
        if (notificationResult.IsFailure)
            return Result.Fail<Guid>(notificationResult.Errors!);
        
        await notificationChannelRepository.AddAsync(notificationResult.Value, cancellationToken);
        return Result.Success(notificationResult.Value.Id);
    }
}