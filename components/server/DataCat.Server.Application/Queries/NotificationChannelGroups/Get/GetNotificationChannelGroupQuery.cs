namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Get;

public sealed record GetNotificationChannelGroupQuery(string Name)
    : IQuery<NotificationChannelGroupResponse>, IAuthorizedQuery;
