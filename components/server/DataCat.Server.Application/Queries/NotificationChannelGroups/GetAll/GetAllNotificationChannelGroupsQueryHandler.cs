namespace DataCat.Server.Application.Queries.NotificationChannelGroups.GetAll;

public sealed class GetAllNotificationChannelGroupsQueryHandler(
    INotificationChannelGroupRepository notificationChannelGroupRepository)
    : IQueryHandler<GetAllNotificationChannelGroupsQuery, List<NotificationChannelGroupResponse>>
{
    public async Task<Result<List<NotificationChannelGroupResponse>>> Handle(GetAllNotificationChannelGroupsQuery request, CancellationToken cancellationToken)
    {
        var result = await notificationChannelGroupRepository.GetAllAsync(cancellationToken);
        return Result.Success(result.Select(x => x.ToResponse()).ToList());
    }
}