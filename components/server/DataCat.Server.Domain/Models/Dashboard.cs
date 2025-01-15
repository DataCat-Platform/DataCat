namespace DataCat.Server.Domain.Models;

public class Dashboard
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public List<Panel> Panels { get; set; } = [];
    
    public required User Owner { get; set; }
    
    public List<User> SharedWith { get; set; } = [];
    
    public required DateTime CreatedAt { get; set; }
    
    public required DateTime UpdatedAt { get; set; }
}