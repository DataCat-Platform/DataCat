namespace DataCat.Server.Application.Queries.User.Get;

public sealed record GetUserQueryHandler(
    IRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<GetUserQuery, Result<UserEntity>>
{
    public async Task<Result<UserEntity>> Handle(GetUserQuery request, CancellationToken token)
    {
        var entity = await userRepository.GetByIdAsync(request.UserId, token);
        return entity is null 
            ? Result.Fail<UserEntity>(UserError.NotFound) 
            : Result<UserEntity>.Success(entity);
    }
}