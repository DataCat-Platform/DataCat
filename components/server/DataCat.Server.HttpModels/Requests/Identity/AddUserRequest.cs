namespace DataCat.Server.HttpModels.Requests.Identity;

public class AddUserRequest
{
    public required string UserName { get; set; }

    public required string Email { get; set; }
    
    public required int RoleId { get; set; }
}