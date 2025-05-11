namespace DataCat.Server.Application.Queries.Users.Get;

public sealed record GetUserQueryHandler(
    IRepository<User, Guid> userRepository)
    : IQueryHandler<GetUserQuery, User>
{
    public async Task<Result<User>> Handle(GetUserQuery request, CancellationToken token)
    {
        var entity = await userRepository.GetByIdAsync(request.UserId, token);
        return entity is null 
            ? Result.Fail<User>(UserError.NotFound) 
            : Result<User>.Success(entity);
    }
}