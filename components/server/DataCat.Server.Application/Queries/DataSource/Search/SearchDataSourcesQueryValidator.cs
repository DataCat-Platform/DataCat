namespace DataCat.Server.Application.Queries.DataSource.Search;

public class SearchDataSourcesQueryValidator : AbstractValidator<SearchDataSourcesQuery>
{
    public SearchDataSourcesQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}