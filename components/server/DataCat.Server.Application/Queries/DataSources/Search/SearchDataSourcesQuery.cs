namespace DataCat.Server.Application.Queries.DataSources.Search;

public sealed record SearchDataSourcesQuery(
    int Page, 
    int PageSize, 
    string? Filter)
    : IRequest<Result<Page<SearchDataSourcesResponse>>>, ISearchQuery, IAuthorizedQuery;
