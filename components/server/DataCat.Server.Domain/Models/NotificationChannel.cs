namespace DataCat.Server.Domain.Models;

public class NotificationChannel
{
    private NotificationChannel(Guid id, NotificationDestination destination, string address)
    {
        Id = id;
        Destination = destination;
        Address = address;
    }

    public Guid Id { get; private set; }

    public NotificationDestination Destination { get; private set; }

    public string Address { get; private set; }

    public static Result<NotificationChannel> Create(Guid id, NotificationDestination? destination, string? address)
    {
        if (destination is null)
        {
            return Result.Fail<NotificationChannel>("Destination cannot be null");
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            return Result.Fail<NotificationChannel>("Address cannot be null or empty");
        }

        return Result.Success(new NotificationChannel(id, destination, address));
    }
}