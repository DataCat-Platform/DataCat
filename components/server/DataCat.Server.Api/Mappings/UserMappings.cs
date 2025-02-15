namespace DataCat.Server.Api.Mappings;

public static class UserMappings
{
    public static AddUserCommand ToAddCommand(this AddUserRequest request)
    {
        return new AddUserCommand
        {
            UserName = request.UserName,
            Email = request.Email,
            RoleId = request.RoleId,
        };
    }

    public static UpdateUserRoleCommand ToUpdateRoleCommand(this UpdateUserRoleRequest request)
    {
        return new UpdateUserRoleCommand
        {
            UserId = request.UserId,
            RoleId = request.RoleId,
        };
    }
    
    public static UpdateUserInfoCommand ToUpdateSettingsCommand(this UpdateUserInfoRequest request)
    {
        return new UpdateUserInfoCommand
        {
            UserId = request.UserId,
            UserName = request.UserName,
            Email = request.Email,
        };
    }

    public static UserResponse ToResponse(this UserEntity entity)
    {
        return new UserResponse
        {
            UserId = entity.Id,
            UserName = entity.Username,
            Email = entity.Email,
            Role = entity.Role.Name,
        };
    }
}