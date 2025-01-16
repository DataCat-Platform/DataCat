namespace DataCat.Server.Domain.Models;

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

    public Result<Alert> Create(
        Guid id, 
        string name, 
        Query? query, 
        string? condition, 
        NotificationChannel? notificationChannel)
    {
        if (condition is null)
        {
            return Result.Fail<Alert>("condition cannot be null");
        }

        if (notificationChannel is null)
        {
            return Result.Fail<Alert>("notificationChannel cannot be null");
        }

        if (query is null)
        {
            return Result.Fail<Alert>("query cannot be null");
        }
        
        return Result.Success(new Alert(id, name, query, condition, notificationChannel));
    }
}