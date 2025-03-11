namespace DataCat.Server.Domain.Core;

public class AlertEntity
{
    private AlertEntity(
        Guid id,
        string? description,
        QueryEntity queryEntity,
        AlertStatus alertStatus,
        NotificationChannelEntity notificationChannelEntity,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        TimeSpan waitTimeBeforeAlerting,
        TimeSpan repeatInterval)
    {
        Id = id;
        Description = description;
        QueryEntity = queryEntity;
        Status = alertStatus;
        NotificationChannelEntity = notificationChannelEntity;
        PreviousExecution = previousExecution;
        NextExecution = nextExecution;
        WaitTimeBeforeAlerting = waitTimeBeforeAlerting;
        RepeatInterval = repeatInterval;
    }
    
    public Guid Id { get; private set; }
    
    public string? Description { get; private set; }
    
    public QueryEntity QueryEntity { get; private set; }
    
    public AlertStatus Status { get; private set; }
    
    public NotificationChannelEntity NotificationChannelEntity { get; private set; }
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
    
    public Result ChangeAlertQuery(QueryEntity? queryEntity)
    {
        if (queryEntity is null)
        {
            return Result.Fail(new ErrorInfo("Invalid query entity")); 
        }
        QueryEntity = queryEntity;
        return Result.Success();
    }

    public static Result<AlertEntity> Create(
        Guid id, 
        string? description, 
        QueryEntity? query, 
        AlertStatus? alertStatus,
        NotificationChannelEntity? notificationChannel,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        TimeSpan waitTimeBeforeAlerting,
        TimeSpan repeatInterval)
    {
        var validationList = new List<Result<AlertEntity>>();

        #region Validation

        if (notificationChannel is null)
        {
            validationList.Add(Result.Fail<AlertEntity>(BaseError.FieldIsNull(nameof(notificationChannel))));
        }
        
        if (alertStatus is null)
        {
            validationList.Add(Result.Fail<AlertEntity>(BaseError.FieldIsNull(nameof(alertStatus))));
        }

        if (query is null)
        {
            validationList.Add(Result.Fail<AlertEntity>(BaseError.FieldIsNull(nameof(query))));
        }

        if (waitTimeBeforeAlerting == TimeSpan.Zero)
        {
            validationList.Add(Result.Fail<AlertEntity>("Wait Time before alerting should be greater than zero"));
        }

        if (repeatInterval == TimeSpan.Zero)
        {
            validationList.Add(Result.Fail<AlertEntity>("Alert repeat interval should be greater than zero"));
        }

        #endregion
        return validationList.Count != 0 
            ? validationList.FoldResults()! 
            : Result.Success(
                new AlertEntity(id, 
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