namespace DataCat.Server.Application.Queries.Alert.Search;

public sealed class SearchAlertsQueryValidator : AbstractValidator<SearchAlertsQuery>
{
    public SearchAlertsQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}