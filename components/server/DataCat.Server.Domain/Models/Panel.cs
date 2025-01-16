namespace DataCat.Server.Domain.Models;

public class Panel
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public PanelType PanelType { get; set; }
    
    public required Query Query { get; set; }
    
    public required Layout Layout { get; set; }
    
    public required Dashboard ParentDashboard { get; set; }
}