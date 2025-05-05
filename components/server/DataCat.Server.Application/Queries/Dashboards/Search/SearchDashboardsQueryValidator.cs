namespace DataCat.Server.Application.Queries.Dashboards.Search;

public sealed class SearchDashboardsQueryValidator : AbstractValidator<SearchDashboardsQuery>
{
    public SearchDashboardsQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}