namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Get;

public class GetNotificationChannelGroupQueryHandler(
    IRepository<NotificationChannelGroup, Guid> repository)
    : IQueryHandler<GetNotificationChannelGroupQuery, NotificationChannelGroupResponse>
{
    public async Task<Result<NotificationChannelGroupResponse>> Handle(GetNotificationChannelGroupQuery request,
        CancellationToken token)
    {
        var entity = await repository.GetByIdAsync(request.Id, token);
        return entity is null
            ? Result.Fail<NotificationChannelGroupResponse>(NotificationChannelGroupError.NotFound(request.Id))
            : Result.Success(entity.ToResponse());
    }
}