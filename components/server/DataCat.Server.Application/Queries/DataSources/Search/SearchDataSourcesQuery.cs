namespace DataCat.Server.Application.Queries.DataSources.Search;

public sealed record SearchDataSourcesQuery(
    int Page, 
    int PageSize, 
    SearchFilters Filters)
    : IQuery<Page<SearchDataSourcesResponse>>, ISearchQuery, IAuthorizedQuery;
