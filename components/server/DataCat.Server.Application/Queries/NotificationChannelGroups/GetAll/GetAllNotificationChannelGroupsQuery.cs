namespace DataCat.Server.Application.Queries.NotificationChannelGroups.GetAll;

public sealed record GetAllNotificationChannelGroupsQuery
    : IQuery<List<NotificationChannelGroupResponse>>, IAuthorizedQuery;
