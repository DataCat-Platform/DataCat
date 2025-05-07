namespace DataCat.Storage.Postgres.Services;

public class PostgresAlertMonitorService(
    IDbConnectionFactory<NpgsqlConnection> Factory,
    UnitOfWork unitOfWork,
    NotificationChannelManager notificationChannelManager)
    : IAlertMonitorService
{
    public async Task<IEnumerable<Alert>> GetAlertsToCheckAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { p_limit = limit };
        var sql = $"""
           SELECT 
               alert.{Public.Alerts.Id}                                          {nameof(AlertSnapshot.Id)},
               alert.{Public.Alerts.Description}                                 {nameof(AlertSnapshot.Description)},
               alert.{Public.Alerts.Template}                                    {nameof(AlertSnapshot.Template)},
               alert.{Public.Alerts.Status}                                      {nameof(AlertSnapshot.Status)},
               alert.{Public.Alerts.RawQuery}                                    {nameof(AlertSnapshot.ConditionQuery)},
               alert.{Public.Alerts.DataSourceId}                                {nameof(AlertSnapshot.DataSourceId)},
               alert.{Public.Alerts.NotificationChannelGroupId}                  {nameof(AlertSnapshot.NotificationChannelGroup)},
               alert.{Public.Alerts.PreviousExecution}                           {nameof(AlertSnapshot.PreviousExecution)},
               alert.{Public.Alerts.NextExecution}                               {nameof(AlertSnapshot.NextExecution)},
               alert.{Public.Alerts.WaitTimeBeforeAlertingInTicks}               {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
               alert.{Public.Alerts.RepeatIntervalInTicks}                       {nameof(AlertSnapshot.RepeatIntervalInTicks)},
               alert.{Public.Alerts.Tags}                                        {nameof(AlertSnapshot.Tags)},
   
               data_source.{Public.DataSources.Id}                                    {nameof(DataSourceSnapshot.Id)},
               data_source.{Public.DataSources.Name}                                  {nameof(DataSourceSnapshot.Name)},
               data_source.{Public.DataSources.TypeId}                                {nameof(DataSourceSnapshot.TypeId)},
               data_source.{Public.DataSources.ConnectionSettings}                    {nameof(DataSourceSnapshot.ConnectionSettings)},
               data_source.{Public.DataSources.Purpose}                               {nameof(DataSourceSnapshot.Purpose)},
   
               notification_channel.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
               notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}   {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
               notification_channel.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
               notification_channel.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)},
               
               notification_channel_group.{Public.NotificationChannelGroups.Id}            {nameof(NotificationChannelGroupSnapshot.Id)},
               notification_channel_group.{Public.NotificationChannelGroups.Name}          {nameof(NotificationChannelGroupSnapshot.Name)}
               
           FROM
               {Public.AlertTable} alert
           JOIN
               {Public.DataSourceTable} data_source ON alert.{Public.Alerts.DataSourceId} = data_source.{Public.DataSources.Id}
           JOIN
               {Public.NotificationChannelGroupTable} notification_channel_group ON notification_channel_group.{Public.NotificationChannelGroups.Id} = alert.{Public.NotificationChannels.NotificationChannelGroupId}
           JOIN
               {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_channel_group.{Public.NotificationChannelGroups.Id}
           WHERE
               alert.{Public.Alerts.Status} = {AlertStatus.InActive.Value}
               AND
               alert.{Public.Alerts.NextExecution} < NOW()
           ORDER BY
               alert.{Public.Alerts.NextExecution}
           LIMIT @p_limit
           FOR UPDATE SKIP LOCKED;
        """;
        
        var connection = await Factory.GetOrCreateConnectionAsync(token);
        
        var alertDictionary = new Dictionary<string, AlertSnapshot>();

        await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelGroupSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannelGroup, notificationChannel) =>
                {
                    if (!alertDictionary.TryGetValue(alert.Id, out var existingAlert))
                    {
                        alert.DataSource = dataSource;
                        alert.NotificationChannelGroup = notificationChannelGroup;

                        alertDictionary.Add(alert.Id, alert);
                        existingAlert = alert;
                    }

                    if (existingAlert.NotificationChannelGroup.Channels.All(c => c.Id != notificationChannel.Id))
                    {
                        existingAlert.NotificationChannelGroup.Channels.Add(notificationChannel);
                    }

                    return existingAlert;
                },
                splitOn: $"{nameof(DataSourceSnapshot.Id)}, {nameof(NotificationChannelGroupSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return alertDictionary.Select(x => x.Value).Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }

    public async Task<IEnumerable<Alert>> GetTriggeredAlertsAsync(int limit = 5, CancellationToken token = default)
    {
        var parameters = new { p_limit = limit };
        
        var sql = $"""
            SELECT 
                alert.{Public.Alerts.Id}                                  {nameof(AlertSnapshot.Id)},
                alert.{Public.Alerts.Description}                         {nameof(AlertSnapshot.Description)},
                alert.{Public.Alerts.Template}                            {nameof(AlertSnapshot.Template)},
                alert.{Public.Alerts.Status}                              {nameof(AlertSnapshot.Status)},
                alert.{Public.Alerts.RawQuery}                            {nameof(AlertSnapshot.ConditionQuery)},
                alert.{Public.Alerts.DataSourceId}                        {nameof(AlertSnapshot.DataSourceId)},
                alert.{Public.Alerts.NotificationChannelGroupId}          {nameof(AlertSnapshot.NotificationChannelGroup)},
                alert.{Public.Alerts.PreviousExecution}                   {nameof(AlertSnapshot.PreviousExecution)},
                alert.{Public.Alerts.NextExecution}                       {nameof(AlertSnapshot.NextExecution)},
                alert.{Public.Alerts.WaitTimeBeforeAlertingInTicks}       {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                alert.{Public.Alerts.RepeatIntervalInTicks}               {nameof(AlertSnapshot.RepeatIntervalInTicks)},
                
                data_source.{Public.DataSources.Id}                     {nameof(DataSourceSnapshot.Id)},
                data_source.{Public.DataSources.Name}                   {nameof(DataSourceSnapshot.Name)},
                data_source.{Public.DataSources.TypeId}                 {nameof(DataSourceSnapshot.TypeId)},
                data_source.{Public.DataSources.ConnectionSettings}     {nameof(DataSourceSnapshot.ConnectionSettings)},
                data_source.{Public.DataSources.Purpose}                {nameof(DataSourceSnapshot.Purpose)},
                
                notification_channel.{Public.NotificationChannels.Id}                            {nameof(NotificationChannelSnapshot.Id)},
                notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}    {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                notification_channel.{Public.NotificationChannels.Settings}                      {nameof(NotificationChannelSnapshot.Settings)},
                notification_channel.{Public.NotificationChannels.DestinationId}                 {nameof(NotificationChannelSnapshot.DestinationId)},
                
                notification_channel_group.{Public.NotificationChannelGroups.Id}           {nameof(NotificationChannelGroupSnapshot.Id)},
                notification_channel_group.{Public.NotificationChannelGroups.Name}         {nameof(NotificationChannelGroupSnapshot.Name)}
           FROM
            {Public.AlertTable} alert
        JOIN
            {Public.DataSourceTable} data_source ON alert.{Public.Alerts.DataSourceId} = data_source.{Public.DataSources.Id}
        JOIN
            {Public.NotificationChannelGroupTable} notification_channel_group ON notification_channel_group.{Public.NotificationChannelGroups.Id} = alert.{Public.NotificationChannels.NotificationChannelGroupId}
        JOIN
            {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_channel_group.{Public.NotificationChannelGroups.Id}
           WHERE
               alert.{Public.Alerts.Status} IN ({AlertStatus.Warning.Value}, {AlertStatus.Fire.Value}, {AlertStatus.Muted.Value})
               AND
               alert.{Public.Alerts.NextExecution} < NOW()
           ORDER BY
               alert.{Public.Alerts.NextExecution}
           LIMIT @p_limit
           FOR UPDATE SKIP LOCKED;
        """;

        var connection = await Factory.GetOrCreateConnectionAsync(token);

        var alertDictionary = new Dictionary<string, AlertSnapshot>();

        await connection
            .QueryAsync<AlertSnapshot, DataSourceSnapshot, NotificationChannelGroupSnapshot, NotificationChannelSnapshot, AlertSnapshot>(
                sql,
                map: (alert, dataSource, notificationChannelGroup, notificationChannel) =>
                {
                    if (!alertDictionary.TryGetValue(alert.Id, out var existingAlert))
                    {
                        alert.DataSource = dataSource;
                        alert.NotificationChannelGroup = notificationChannelGroup;

                        alertDictionary.Add(alert.Id, alert);
                        existingAlert = alert;
                    }

                    if (existingAlert.NotificationChannelGroup.Channels.All(c => c.Id != notificationChannel.Id))
                    {
                        existingAlert.NotificationChannelGroup.Channels.Add(notificationChannel);
                    }

                    return existingAlert;
                },
                splitOn: $"{nameof(DataSourceSnapshot.Id)}, {nameof(NotificationChannelGroupSnapshot.Id)}, {nameof(NotificationChannelSnapshot.Id)}",
                param: parameters,
                transaction: unitOfWork.Transaction);

        return alertDictionary.Select(x => x.Value).Select(x => x.RestoreFromSnapshot(notificationChannelManager));
    }
}