namespace DataCat.Server.Application.Commands.User.UpdateInfo;

public sealed class UpdateUserInfoCommandHandler(
    IDefaultRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<UpdateUserInfoCommand, Result>
{
    public async Task<Result> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);
        if (user is null)
            return Result.Fail(UserError.NotFound);
        
        user.UpdateEmail(request.Email);
        user.UpdateUsername(request.UserName);
        
        await userRepository.UpdateAsync(user, cancellationToken); 
        return Result.Success();
    }
}