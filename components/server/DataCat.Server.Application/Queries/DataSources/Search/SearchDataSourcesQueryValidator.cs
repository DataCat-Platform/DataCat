namespace DataCat.Server.Application.Queries.DataSources.Search;

public class SearchDataSourcesQueryValidator : AbstractValidator<SearchDataSourcesQuery>
{
    public SearchDataSourcesQueryValidator()
    {
        Include(new SearchQueryValidator());
    }
}