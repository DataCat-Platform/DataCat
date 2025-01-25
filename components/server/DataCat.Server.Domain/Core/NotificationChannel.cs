namespace DataCat.Server.Domain.Core;

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
        var validationList = new List<Result<NotificationChannel>>();

        #region Validation

        if (destination is null)
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(destination))));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(address))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()!
            : Result.Success(new NotificationChannel(id, destination!, address!));
    }
}