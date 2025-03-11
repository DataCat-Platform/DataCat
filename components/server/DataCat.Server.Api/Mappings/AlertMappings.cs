using DataCat.Server.Application.Commands.Alert.Update;

namespace DataCat.Server.Api.Mappings;

public static class AlertMappings
{
    public static AddAlertCommand ToAddCommand(this AddAlertRequest request)
    {
        return new AddAlertCommand
        {
            Description = request.Description,
            RawQuery = request.RawQuery,
            DataSourceId = request.DataSourceId,
            NotificationChannelId = request.NotificationChannelId,
            WaitTimeBeforeAlerting = request.WaitTimeBeforeAlerting,
            RepeatInterval = request.RepeatInterval
        };
    }

    public static UpdateAlertQueryCommand ToUpdateCommand(this UpdateAlertRequest request, string alertId)
    {
        return new UpdateAlertQueryCommand
        {
            AlertId = alertId,
            Description = request.Description,
            RawQuery = request.RawQuery,
            DataSourceId = request.DataSourceId,
            NotificationChannelId = request.NotificationChannelId
        };
    }

    public static AlertResponse ToResponse(this AlertEntity alert)
    {
        return new AlertResponse
        {
            Id = alert.Id,
            Description = alert.Description,
            RawQuery = alert.QueryEntity.RawQuery,
            Status = alert.Status.Name,
            DataSource = alert.QueryEntity.DataSourceEntity.ToResponse(),
            NotificationChannel = alert.NotificationChannelEntity.ToResponse(),
            WaitTimeBeforeAlerting = alert.WaitTimeBeforeAlerting,
            RepeatInterval = alert.RepeatInterval,
            PreviousExecutionTime = alert.PreviousExecution.DateTime,
            NextExecutionTime = alert.NextExecution.DateTime,
        };
    }
}