namespace DataCat.Server.Domain.Core;

public class Alert
{
    private Alert(
        Guid id,
        string name,
        Query query,
        string condition,
        NotificationChannel notificationChannel)
    {
        Id = id;
        Name = name;
        Query = query;
        Condition = condition;
        NotificationChannel = notificationChannel;
    }
    
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public Query Query { get; private set; }

    /// <summary>
    /// For example, avg(x) > 90
    /// </summary>
    public string Condition { get; private set; }
    
    public NotificationChannel NotificationChannel { get; private set; }

    public static Result<Alert> Create(
        Guid id, 
        string name, 
        Query? query, 
        string? condition, 
        NotificationChannel? notificationChannel)
    {
        var validationList = new List<Result<Alert>>();

        #region Validation

        if (condition is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(condition))));
        }

        if (notificationChannel is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(notificationChannel))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(query))));
        }

        #endregion
        
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(new Alert(id, name, query!, condition!, notificationChannel!));
    }
}