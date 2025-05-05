namespace DataCat.Server.Application.Queries.Plugins.Search;

public sealed class SearchPluginValidator : AbstractValidator<SearchPluginsQuery>
{
    public SearchPluginValidator()
    {
        Include(new SearchQueryValidator());
    }
}