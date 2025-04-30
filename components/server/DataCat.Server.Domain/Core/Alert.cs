namespace DataCat.Server.Domain.Core;

public class Alert
{
    private Alert(
        Guid id,
        string? description,
        Query query,
        AlertStatus alertStatus,
        NotificationChannel notificationChannel,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        TimeSpan waitTimeBeforeAlerting,
        TimeSpan repeatInterval)
    {
        Id = id;
        Description = description;
        Query = query;
        Status = alertStatus;
        NotificationChannel = notificationChannel;
        PreviousExecution = previousExecution;
        NextExecution = nextExecution;
        WaitTimeBeforeAlerting = waitTimeBeforeAlerting;
        RepeatInterval = repeatInterval;
    }
    
    public Guid Id { get; private set; }
    
    public string? Description { get; private set; }
    
    public Query Query { get; private set; }
    
    public AlertStatus Status { get; private set; }
    
    public NotificationChannel NotificationChannel { get; private set; }
    public DateTimeUtc PreviousExecution { get; private set; }
    public DateTimeUtc NextExecution { get; private set; }
    public TimeSpan WaitTimeBeforeAlerting { get; private set; }
    public TimeSpan RepeatInterval { get; private set; }

    public void ChangeDescription(string? description) => Description = description;
    public void ChangeWaitTime(TimeSpan waitTimeBeforeAlerting) => WaitTimeBeforeAlerting = waitTimeBeforeAlerting;
    public void ChangeRepeatInterval(TimeSpan repeatInterval) => RepeatInterval = repeatInterval;

    public void ResetAlert()
    {
        Status = AlertStatus.InActive;
        CommitAlertExecution();
    }

    public void SetFire()
    {
        Status = AlertStatus.Fire;
        CommitAlertExecution();
    }
    
    public void SetWarningStatus()
    {
        Status = AlertStatus.Warning;
        var nextExecution = DateTime.UtcNow.Add(WaitTimeBeforeAlerting);
        UpdateExecutionTimes(nextExecution);
    }

    public void CommitAlertExecution()
    {
        var nextExecution = DateTime.UtcNow.Add(RepeatInterval);
        UpdateExecutionTimes(nextExecution);
    }

    private void UpdateExecutionTimes(DateTimeUtc nextExecution)
    {
        PreviousExecution = DateTime.UtcNow;
        NextExecution = nextExecution;
    }
    
    public Result MuteAlert(DateTimeUtc nextExecutionAt)
    {
        if (nextExecutionAt < NextExecution)
        {
            return Result.Fail(AlertError.InvalidNextExecutionTime);
        }
        UpdateExecutionTimes(nextExecutionAt);
        Status = AlertStatus.Muted;
        return Result.Success();
    }
    
    public Result ChangeAlertQuery(Query? queryEntity)
    {
        if (queryEntity is null)
        {
            return Result.Fail(new ErrorInfo("Invalid query entity")); 
        }
        Query = queryEntity;
        return Result.Success();
    }

    public static Result<Alert> Create(
        Guid id, 
        string? description, 
        Query? query, 
        AlertStatus? alertStatus,
        NotificationChannel? notificationChannel,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        TimeSpan waitTimeBeforeAlerting,
        TimeSpan repeatInterval)
    {
        var validationList = new List<Result<Alert>>();

        #region Validation

        if (notificationChannel is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(notificationChannel))));
        }
        
        if (alertStatus is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(alertStatus))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(query))));
        }

        if (waitTimeBeforeAlerting == TimeSpan.Zero)
        {
            validationList.Add(Result.Fail<Alert>("Wait Time before alerting should be greater than zero"));
        }

        if (repeatInterval == TimeSpan.Zero)
        {
            validationList.Add(Result.Fail<Alert>("Alert repeat interval should be greater than zero"));
        }

        #endregion
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(
                new Alert(id, 
                    description, 
                    query!, 
                    alertStatus!, 
                    notificationChannel!,
                    previousExecution,
                    nextExecution,
                    waitTimeBeforeAlerting,
                    repeatInterval));
    }
}