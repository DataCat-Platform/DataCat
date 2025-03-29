namespace DataCat.Server.Application.Queries.DataSource.Search;

public sealed record SearchDataSourcesQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IRequest<Result<Page<SearchDataSourcesResponse>>>, ISearchQuery, IAuthorizedQuery;
