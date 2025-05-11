namespace DataCat.Server.Application.Queries.Alerts.Search;

public sealed class SearchAlertsQueryValidator : AbstractValidator<SearchAlertsQuery>
{
    public SearchAlertsQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}