namespace DataCat.Server.Application.Queries.NotificationChannel.Search;

public sealed class SearchNotificationChannelsQueryValidator : AbstractValidator<SearchNotificationChannelsQuery>
{
    public SearchNotificationChannelsQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}