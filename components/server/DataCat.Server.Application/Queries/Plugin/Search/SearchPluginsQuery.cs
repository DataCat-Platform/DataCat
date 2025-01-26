namespace DataCat.Server.Application.Queries.Plugin.Search;

public sealed record SearchPluginsQuery(
    int Page, 
    int PageSize, 
    string? Filter) : IRequest<Result<IEnumerable<PluginEntity>>>, ISearchQuery;
