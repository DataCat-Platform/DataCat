namespace DataCat.Server.Domain.Models;

public class NotificationChannel
{
    public Guid Id { get; set; }
    
    public required NotificationDestination Destination { get; set; }
    
    public required string Address { get; set; }
}