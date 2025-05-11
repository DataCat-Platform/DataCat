namespace DataCat.Server.Domain.Core;

public sealed class NotificationChannelGroup
{
    private NotificationChannelGroup(
        Guid id,
        string name,
        List<NotificationChannel> notificationChannels,
        Guid namespaceId)
    {
        Id = id;
        Name = name;
        NamespaceId = namespaceId;
        _notificationChannels = notificationChannels;
    }
    
    public Guid Id { get; }
    public string Name { get; }
    public Guid NamespaceId { get; }

    private readonly List<NotificationChannel> _notificationChannels;
    public IReadOnlyCollection<NotificationChannel> NotificationChannels => _notificationChannels.AsReadOnly();
    
    public static Result<NotificationChannelGroup> Create(
        Guid id, 
        string name,
        List<NotificationChannel> notificationChannels,
        Guid namespaceId)
    {
        var validationList = new List<Result<NotificationChannelGroup>>();

        #region Validation

        if (string.IsNullOrWhiteSpace(name))
        {
            validationList.Add(Result.Fail<NotificationChannelGroup>(BaseError.FieldIsNull(nameof(name))));
        }

        #endregion

        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new NotificationChannelGroup(id, name, notificationChannels ?? [], namespaceId));
    }
}