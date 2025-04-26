namespace DataCat.Server.Domain.Core;

public class NotificationChannel
{
    private NotificationChannel(
        Guid id, 
        BaseNotificationOption notificationOption)
    {
        Id = id;
        NotificationOption = notificationOption;
    }

    public Guid Id { get; private set; }

    public BaseNotificationOption NotificationOption { get; private set; }

    public void ChangeConfiguration(BaseNotificationOption settings)
    {
        NotificationOption = settings;
    }

    public static Result<NotificationChannel> Create(Guid id, 
        BaseNotificationOption? settings)
    {
        var validationList = new List<Result<NotificationChannel>>();

        #region Validation

        if (settings is null)
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(settings))));
        }

        if (settings?.NotificationDestination is null)
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(BaseNotificationOption.NotificationDestination))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()!
            : Result.Success(new NotificationChannel(id, settings!));
    }
}