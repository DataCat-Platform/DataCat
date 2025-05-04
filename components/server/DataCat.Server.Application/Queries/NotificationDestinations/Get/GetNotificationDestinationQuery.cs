namespace DataCat.Server.Application.Queries.NotificationDestinations.Get;

public sealed record GetNotificationDestinationQuery(string Name)
    : IQuery<GetNotificationDestinationResponse>, IAuthorizedQuery;
