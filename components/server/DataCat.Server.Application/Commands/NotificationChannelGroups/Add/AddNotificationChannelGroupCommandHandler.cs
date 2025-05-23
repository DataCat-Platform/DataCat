namespace DataCat.Server.Application.Commands.NotificationChannelGroups.Add;

public sealed class AddNotificationChannelGroupCommandHandler(
    IRepository<NotificationChannelGroup, Guid> repository,
    NamespaceContext namespaceContext) 
    : ICommandHandler<AddNotificationChannelGroupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddNotificationChannelGroupCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();

        var notificationChannelGroup = NotificationChannelGroup.Create(
            id,
            request.Name,
            [],
            namespaceContext.GetNamespaceId());
        
        if (notificationChannelGroup.IsFailure)
            return Result.Fail<Guid>(notificationChannelGroup.Errors!);
        
        await repository.AddAsync(notificationChannelGroup.Value, cancellationToken);
        return Result.Success(id);
    }
}