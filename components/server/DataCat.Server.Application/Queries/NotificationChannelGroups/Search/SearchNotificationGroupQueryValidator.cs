namespace DataCat.Server.Application.Queries.NotificationChannelGroups.Search;

public sealed class SearchNotificationGroupQueryValidator : AbstractValidator<SearchNotificationGroupQuery>
{
    public SearchNotificationGroupQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}