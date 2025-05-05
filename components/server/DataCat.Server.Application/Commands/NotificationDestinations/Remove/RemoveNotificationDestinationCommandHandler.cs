namespace DataCat.Server.Application.Commands.NotificationDestinations.Remove;

public sealed class RemoveNotificationDestinationCommandHandler(
    INotificationDestinationRepository notificationDestinationRepository) 
    : ICommandHandler<RemoveNotificationDestinationCommand>
{
    public async Task<Result> Handle(RemoveNotificationDestinationCommand request, CancellationToken cancellationToken)
    {
        var notificationDestination = await notificationDestinationRepository.GetByNameAsync(request.Name, cancellationToken);
        if (notificationDestination is null)
            return Result.Fail(NotificationDestinationError.NotFound(request.Name));
        
        await notificationDestinationRepository.DeleteAsync(notificationDestination.Id, cancellationToken);
        return Result.Success();
    }
}