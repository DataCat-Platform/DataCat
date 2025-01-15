namespace DataCat.Server.Domain.Models;

public class Alert
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required Query Query { get; set; }

    /// <summary>
    /// For example, avg(x) > 90
    /// </summary>
    public required string Condition { get; set; }
    
    public required NotificationChannel NotificationChannel { get; set; }
}