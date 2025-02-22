namespace DataCat.Server.Domain.Core;

public class AlertEntity
{
    private AlertEntity(
        Guid id,
        string description,
        QueryEntity queryEntity,
        NotificationChannelEntity notificationChannelEntity)
    {
        Id = id;
        Description = description;
        QueryEntity = queryEntity;
        NotificationChannelEntity = notificationChannelEntity;
    }
    
    public Guid Id { get; private set; }
    
    public string Description { get; private set; }
    
    public QueryEntity QueryEntity { get; private set; }
    
    public NotificationChannelEntity NotificationChannelEntity { get; private set; }
    
    public void ChangeAlertQuery(QueryEntity queryEntity) => QueryEntity = queryEntity;
    public void ChangeDescription(string? description) => Description = description;

    public static Result<AlertEntity> Create(
        Guid id, 
        string description, 
        QueryEntity? query, 
        NotificationChannelEntity? notificationChannel)
    {
        var validationList = new List<Result<AlertEntity>>();

        #region Validation

        if (notificationChannel is null)
        {
            validationList.Add(Result.Fail<AlertEntity>(BaseError.FieldIsNull(nameof(notificationChannel))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<AlertEntity>(BaseError.FieldIsNull(nameof(query))));
        }

        #endregion
        
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new AlertEntity(id, description, query!, notificationChannel!));
    }
}