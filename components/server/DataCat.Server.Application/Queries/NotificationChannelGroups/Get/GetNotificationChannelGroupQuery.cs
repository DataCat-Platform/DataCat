namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Get;

public sealed record GetNotificationChannelGroupQuery(Guid Id)
    : IQuery<NotificationChannelGroupResponse>, IAuthorizedQuery;
