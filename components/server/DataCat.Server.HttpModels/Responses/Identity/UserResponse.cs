namespace DataCat.Server.HttpModels.Responses.Identity;

public class UserResponse
{
    public required Guid UserId { get; set; }
    
    public required string UserName { get; set; }

    public required string Email { get; set; }
    
    public required string Role { get; set; }
}