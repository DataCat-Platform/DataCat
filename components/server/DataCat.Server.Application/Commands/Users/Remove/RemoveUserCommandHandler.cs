namespace DataCat.Server.Application.Commands.Users.Remove;

[Obsolete("Outdated feature")]
public sealed class RemoveUserCommandHandler(
    IUserRepository userRepository)
    : ICommandHandler<RemoveUserCommand>
{
    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken token)
    {
        var id = Guid.Parse(request.UserId);

        await userRepository.DeleteAsync(id, token);
        return Result.Success();
    }
}