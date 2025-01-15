namespace DataCat.Server.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    
    public required string Username { get; set; }
    
    public required string Email { get; set; }
    
    public required UserRole Role { get; set; }
}