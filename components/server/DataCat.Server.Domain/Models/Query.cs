namespace DataCat.Server.Domain.Models;

public class Query
{
    public Guid Id { get; set; }
    
    public required DataSource DataSource { get; set; }
    
    public required string RawQuery { get; set; }
    
    public required TimeRange TimeRange { get; set; }
}