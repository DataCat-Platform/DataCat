namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Search;

public sealed record SearchNotificationGroupQuery(SearchFilters Filters, int Page, int PageSize)
    : IQuery<NotificationChannelGroupResponse>, ISearchQuery, IAuthorizedQuery;
