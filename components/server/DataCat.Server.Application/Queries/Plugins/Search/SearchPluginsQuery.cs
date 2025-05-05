namespace DataCat.Server.Application.Queries.Plugins.Search;

public sealed record SearchPluginsQuery(
    int Page, 
    int PageSize, 
    SearchFilters Filters)
    : IQuery<Page<SearchPluginsResponse>>, ISearchQuery, IAdminRequest;
