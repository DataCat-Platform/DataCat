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
            NotificationChannelId = request.NotificationChannelId
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
            DataSource = alert.QueryEntity.DataSourceEntity.ToResponse(),
            NotificationChannel = alert.NotificationChannelEntity.ToResponse()
        };
    }
}