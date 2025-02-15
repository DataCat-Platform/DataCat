using DataCat.Server.Application.Commands.User.UpdateInfo;

namespace DataCat.Server.Application.Commands.User.UpdateRole;

public sealed class UpdateUserRoleCommandHandler(
    IDefaultRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<UpdateUserRoleCommand, Result>
{
    public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId), cancellationToken);
        if (user is null)
            return Result.Fail(UserError.NotFound);
        
        var role = UserRole.FromValue(request.RoleId);
        if (role is null)
            return Result.Fail<Guid>(UserError.InvalidUserRole);
        
        user.UpdateRole(role);
        
        await userRepository.UpdateAsync(user, cancellationToken); 
        return Result.Success();
    }
}