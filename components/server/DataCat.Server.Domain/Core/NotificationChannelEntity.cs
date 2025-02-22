namespace DataCat.Server.Domain.Core;

public class NotificationChannelEntity
{
    private NotificationChannelEntity(Guid id, NotificationDestination destination, string settings)
    {
        Id = id;
        Destination = destination;
        Settings = settings;
    }

    public Guid Id { get; private set; }

    public NotificationDestination Destination { get; private set; }

    public string Settings { get; private set; }

    public void ChangeConfiguration(NotificationDestination destination, string settings)
    {
        Destination = destination;
        Settings = settings;
    }

    public static Result<NotificationChannelEntity> Create(Guid id, NotificationDestination? destination, string? settings)
    {
        var validationList = new List<Result<NotificationChannelEntity>>();

        #region Validation

        if (destination is null)
        {
            validationList.Add(Result.Fail<NotificationChannelEntity>(BaseError.FieldIsNull(nameof(destination))));
        }

        if (string.IsNullOrWhiteSpace(settings))
        {
            validationList.Add(Result.Fail<NotificationChannelEntity>(BaseError.FieldIsNull(nameof(settings))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()!
            : Result.Success(new NotificationChannelEntity(id, destination!, settings!));
    }
}