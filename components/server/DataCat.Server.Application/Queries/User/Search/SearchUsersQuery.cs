namespace DataCat.Server.Application.Queries.User.Search;

public sealed record SearchUsersQuery(
    int Page, 
    int PageSize, 
    string? Filter) : IRequest<Result<List<UserEntity>>>, ISearchQuery;
