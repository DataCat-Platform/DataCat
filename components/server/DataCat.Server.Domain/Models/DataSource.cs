namespace DataCat.Server.Domain.Models;

public class DataSource
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public DataSourceType DataSourceType { get; set; }
    
    public required string ConnectionString { get; set; }
}