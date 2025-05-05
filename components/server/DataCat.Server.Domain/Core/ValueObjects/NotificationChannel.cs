namespace DataCat.Server.Domain.Core.ValueObjects;

public sealed class NotificationChannel
{
    private NotificationChannel(
        int? id,
        Guid notificationChannelGroupId,
        BaseNotificationOption notificationOption)
    {
        Id = id ?? 0;
        NotificationChannelGroupId = notificationChannelGroupId;
        NotificationOption = notificationOption;
    }

    public int Id { get; private set; }
    public Guid NotificationChannelGroupId { get; }

    public BaseNotificationOption NotificationOption { get; private set; }

    public void ChangeConfiguration(BaseNotificationOption settings)
    {
        NotificationOption = settings;
    }

    public static Result<NotificationChannel> Create(
        Guid notificationChannelGroupId,
        BaseNotificationOption? notificationOption,
        int? id = null)
    {
        var validationList = new List<Result<NotificationChannel>>();

        #region Validation

        if (notificationOption is null)
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(notificationOption))));
        }

        if (notificationOption?.NotificationDestination is null)
        {
            validationList.Add(Result.Fail<NotificationChannel>(BaseError.FieldIsNull(nameof(BaseNotificationOption.NotificationDestination))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()!
            : Result.Success(new NotificationChannel(id, notificationChannelGroupId, notificationOption!));
    }
}