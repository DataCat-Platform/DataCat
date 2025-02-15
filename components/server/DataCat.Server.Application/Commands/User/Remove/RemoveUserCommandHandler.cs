namespace DataCat.Server.Application.Commands.User.Remove;

public sealed class RemoveUserCommandHandler(
    IDefaultRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<RemoveUserCommand, Result>
{
    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.UserId);

        await userRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}