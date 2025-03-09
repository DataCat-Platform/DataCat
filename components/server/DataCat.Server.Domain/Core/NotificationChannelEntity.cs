namespace DataCat.Server.Domain.Core;

public class NotificationChannelEntity
{
    private NotificationChannelEntity(
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

    public static Result<NotificationChannelEntity> Create(Guid id, NotificationDestination? destination, BaseNotificationOption? settings)
    {
        var validationList = new List<Result<NotificationChannelEntity>>();

        #region Validation

        if (destination is null)
        {
            validationList.Add(Result.Fail<NotificationChannelEntity>(BaseError.FieldIsNull(nameof(destination))));
        }

        if (settings is null)
        {
            validationList.Add(Result.Fail<NotificationChannelEntity>(BaseError.FieldIsNull(nameof(settings))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()!
            : Result.Success(new NotificationChannelEntity(id, settings!));
    }
}