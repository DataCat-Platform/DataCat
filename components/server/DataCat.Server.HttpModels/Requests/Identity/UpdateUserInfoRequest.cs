namespace DataCat.Server.HttpModels.Requests.Identity;

public class UpdateUserInfoRequest
{
    public required string UserId { get; set; }
    
    public required string UserName { get; set; }

    public required string Email { get; set; }
}