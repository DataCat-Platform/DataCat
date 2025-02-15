namespace DataCat.Server.Application.Queries.Dashboard.Search;

public sealed class SearchDashboardsQueryValidator : AbstractValidator<SearchDashboardsQuery>
{
    public SearchDashboardsQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}