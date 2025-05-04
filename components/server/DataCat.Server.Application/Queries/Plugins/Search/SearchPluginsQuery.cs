namespace DataCat.Server.Application.Queries.Plugins.Search;

public sealed record SearchPluginsQuery(
    int Page, 
    int PageSize, 
    string? Filter) : IQuery<Page<SearchPluginsResponse>>, ISearchQuery, IAdminRequest;
