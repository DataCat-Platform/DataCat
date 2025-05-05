namespace DataCat.Server.Domain.Core;

public class Alert
{
    private Alert(
        Guid id,
        string Template,
        Query conditionQuery,
        AlertStatus alertStatus,
        NotificationChannelGroup notificationChannelGroup,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        AlertSchedule schedule,
        List<Tag> tags)
    {
        Id = id;
        this.Template = Template;
        ConditionQuery = conditionQuery;
        Status = alertStatus;
        NotificationChannelGroup = notificationChannelGroup;
        PreviousExecution = previousExecution;
        NextExecution = nextExecution;
        Schedule = schedule;
        _tags = tags;
    }
    
    public Guid Id { get; private set; }
    
    public string? Template { get; private set; }
    
    public Query ConditionQuery { get; private set; }
    
    public AlertStatus Status { get; private set; }
    
    public NotificationChannelGroup NotificationChannelGroup { get; private set; }
    public DateTimeUtc PreviousExecution { get; private set; }
    public DateTimeUtc NextExecution { get; private set; }
    public AlertSchedule Schedule { get; private set; }
    private readonly List<Tag> _tags;
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

    public void ChangeDescription(string? description) => Template = description;
    public void ChangeWaitTime(TimeSpan waitTimeBeforeAlerting)
    {
        Schedule = new AlertSchedule(waitTimeBeforeAlerting, Schedule.RepeatInterval);
    }

    public void ChangeRepeatInterval(TimeSpan repeatInterval)
    {
        Schedule = new AlertSchedule(Schedule.WaitTimeBeforeAlerting, repeatInterval);
    } 

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
        var nextExecution = DateTime.UtcNow.Add(Schedule.WaitTimeBeforeAlerting);
        UpdateExecutionTimes(nextExecution);
    }

    public void CommitAlertExecution()
    {
        var nextExecution = DateTime.UtcNow.Add(Schedule.RepeatInterval);
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
        ConditionQuery = queryEntity;
        return Result.Success();
    }

    public static Result<Alert> Create(
        Guid id, 
        string? template, 
        Query? query, 
        AlertStatus? alertStatus,
        NotificationChannelGroup? notificationChannelGroup,
        DateTimeUtc previousExecution,
        DateTimeUtc nextExecution,
        TimeSpan waitTimeBeforeAlerting,
        TimeSpan repeatInterval,
        List<Tag> tags)
    {
        var validationList = new List<Result<Alert>>();

        #region Validation

        if (notificationChannelGroup is null)
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(notificationChannelGroup))));
        }

        if (string.IsNullOrWhiteSpace(template))
        {
            validationList.Add(Result.Fail<Alert>(BaseError.FieldIsNull(nameof(template))));
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
                    template!, 
                    query!, 
                    alertStatus!, 
                    notificationChannelGroup!,
                    previousExecution,
                    nextExecution,
                    new AlertSchedule(waitTimeBeforeAlerting, repeatInterval),
                    tags));
    }
}