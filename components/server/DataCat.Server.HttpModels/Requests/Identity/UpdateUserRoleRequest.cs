namespace DataCat.Server.HttpModels.Requests.Identity;

public class UpdateUserRoleRequest
{
    public required string UserId { get; set; }
    
    public required int RoleId { get; set; }
}