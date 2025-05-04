namespace DataCat.Server.Application.Queries.DataSources.Search;

public sealed record SearchDataSourcesQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IQuery<Page<SearchDataSourcesResponse>>, ISearchQuery, IAuthorizedQuery;
