namespace DataCat.Server.Application.Commands.NotificationChannels.Add;

public sealed class AddNotificationCommandHandler(
    INotificationDestinationRepository notificationDestinationRepository,
    INotificationChannelRepository notificationChannelRepository,
    INotificationChannelGroupRepository notificationChannelGroupRepository,
    NotificationChannelManager notificationChannelManager,
    NamespaceContext namespaceContext)
    : ICommandHandler<AddNotificationCommand, int>
{
    public async Task<Result<int>> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        var destination = await notificationDestinationRepository.GetByNameAsync(request.DestinationTypeName, cancellationToken);
        if (destination is null)
            return Result.Fail<int>(NotificationChannelError.DestinationNotSupported);

        var factory = notificationChannelManager.GetNotificationChannelFactory(destination);
        
        var notificationOptionResult = factory.Create(destination, request.Settings);
        if (notificationOptionResult.IsFailure)
            return Result.Fail<int>(notificationOptionResult.Errors!);
        
        var notificationChannelGroup = await notificationChannelGroupRepository.GetByName(request.NotificationChannelGroupName, cancellationToken);
        if (notificationChannelGroup is null)
            return Result.Fail<int>(NotificationChannelGroupError.NotFound(request.NotificationChannelGroupName));

        var notificationResult = NotificationChannel.Create(
            notificationChannelGroup.Id,
            notificationOptionResult.Value,
            namespaceContext.GetNamespaceId());
        
        if (notificationResult.IsFailure)
            return Result.Fail<int>(notificationResult.Errors!);
        
        var id = await notificationChannelRepository.AddReturningIdAsync(notificationResult.Value, cancellationToken);
        return Result.Success(id);
    }
}