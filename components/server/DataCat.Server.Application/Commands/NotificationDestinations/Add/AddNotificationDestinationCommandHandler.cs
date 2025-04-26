namespace DataCat.Server.Application.Commands.NotificationDestinations.Add;

public sealed class AddNotificationDestinationCommandHandler(
    INotificationDestinationRepository notificationDestinationRepository)
    : IRequestHandler<AddNotificationDestinationCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddNotificationDestinationCommand request, CancellationToken cancellationToken)
    {
        var notificationDestination = await notificationDestinationRepository.GetByNameAsync(request.Name, cancellationToken);
        if (notificationDestination is not null)
            return Result.Success(notificationDestination.Id);
        
        var destinationResult = NotificationDestination.Create(request.Name);
        if (destinationResult.IsFailure)
            return Result.Fail<int>(destinationResult.Errors!);
        
        var id = await notificationDestinationRepository.AddAsync(destinationResult.Value, cancellationToken);
        return Result.Success(id);
    }
}