namespace DataCat.Server.Application.Commands.User.Add;

public sealed class AddUserCommandHandler(
    IDefaultRepository<UserEntity, Guid> userRepository)
    : IRequestHandler<AddUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var role = UserRole.FromValue(request.RoleId);
        if (role is null)
            return Result.Fail<Guid>(UserError.InvalidUserRole);
        
        var id = Guid.NewGuid();
        
        var userResult = UserEntity.Create(id, request.UserName, request.Email, role);
        await userRepository.AddAsync(userResult.Value, cancellationToken); 

        return Result.Success(id);
    }
}