namespace DataCat.Server.Application.Queries.User.Search;

public sealed class SearchUsersQueryHandler(
    IDefaultRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<SearchUsersQuery, Result<List<UserEntity>>>
{
    public async Task<Result<List<UserEntity>>> Handle(SearchUsersQuery request, CancellationToken token)
    {
        var result = await userRepository
            .SearchAsync(request.Filter, request.Page, request.PageSize, token)
            .ToListAsync(token);
        return Result.Success(result);
    }
}