namespace DataCat.Storage.Postgres.Sql;

public static class AlertSql
{
    public static class Select
    {
        public const string GetById = $@"
             SELECT
                {Public.Alerts.Id}                                {nameof(AlertSnapshot.Id)},
                {Public.Alerts.Description}                       {nameof(AlertSnapshot.Description)},
                {Public.Alerts.Status}                            {nameof(AlertSnapshot.Status)},
                {Public.Alerts.RawQuery}                          {nameof(AlertSnapshot.RawQuery)},
                {Public.Alerts.DataSourceId}                      {nameof(AlertSnapshot.DataSourceId)},
                {Public.Alerts.NotificationChannelId}             {nameof(AlertSnapshot.NotificationChannelId)},
                {Public.Alerts.PreviousExecution}                 {nameof(AlertSnapshot.PreviousExecution)},
                {Public.Alerts.NextExecution}                     {nameof(AlertSnapshot.NextExecution)},
                {Public.Alerts.WaitTimeBeforeAlertingInTicks}     {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                {Public.Alerts.RepeatIntervalInTicks}             {nameof(AlertSnapshot.RepeatIntervalInTicks)},
                
                {Public.NotificationChannels.Id}                  {nameof(NotificationChannelSnapshot.Id)},
                {Public.NotificationChannels.DestinationId}       {nameof(NotificationChannelSnapshot.DestinationId)},
                {Public.NotificationChannels.Settings}            {nameof(NotificationChannelSnapshot.Settings)},
                
                {Public.DataSources.Id}                          {nameof(DataSourceSnapshot.Id)},
                {Public.DataSources.Name}                        {nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}                      {nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionString}            {nameof(DataSourceSnapshot.ConnectionString)}

             FROM 
                 {Public.AlertTable} a
             JOIN
                 {Public.NotificationTable} n ON a.{Public.Alerts.NotificationChannelId} = n.{Public.NotificationChannels.Id}
             JOIN
                 {Public.DataSourceTable} d ON a.{Public.Alerts.DataSourceId} = d.{Public.DataSources.Id}
             WHERE {Public.Alerts.Id} = @p_alert_id
             ";
        
        public const string SearchAlertsTotalCount =
            $"""
             SELECT
                COUNT(*)
             FROM 
                 {Public.AlertTable} a
             WHERE a.{Public.Alerts.Description} ILIKE @p_description
             """;
        
        public const string SearchAlerts =
            $"""
             SELECT
                {Public.Alerts.Id}                                 {nameof(AlertSnapshot.Id)},
                {Public.Alerts.Description}                        {nameof(AlertSnapshot.Description)},
                {Public.Alerts.Status}                             {nameof(AlertSnapshot.Status)},
                {Public.Alerts.RawQuery}                           {nameof(AlertSnapshot.RawQuery)},
                {Public.Alerts.DataSourceId}                       {nameof(AlertSnapshot.DataSourceId)},
                {Public.Alerts.NotificationChannelId}              {nameof(AlertSnapshot.NotificationChannelId)},
                {Public.Alerts.PreviousExecution}                  {nameof(AlertSnapshot.PreviousExecution)},
                {Public.Alerts.NextExecution}                      {nameof(AlertSnapshot.NextExecution)},
                {Public.Alerts.WaitTimeBeforeAlertingInTicks}      {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                {Public.Alerts.RepeatIntervalInTicks}              {nameof(AlertSnapshot.RepeatIntervalInTicks)},
                
                {Public.NotificationChannels.Id}                   {nameof(NotificationChannelSnapshot.Id)},
                {Public.NotificationChannels.DestinationId}        {nameof(NotificationChannelSnapshot.DestinationId)},
                {Public.NotificationChannels.Settings}             {nameof(NotificationChannelSnapshot.Settings)},
                
                {Public.DataSources.Id}                            {nameof(DataSourceSnapshot.Id)},
                {Public.DataSources.Name}                          {nameof(DataSourceSnapshot.Name)},
                {Public.DataSources.TypeId}                        {nameof(DataSourceSnapshot.TypeId)},
                {Public.DataSources.ConnectionString}              {nameof(DataSourceSnapshot.ConnectionString)}
             
             FROM 
                 {Public.AlertTable} a
             JOIN
                 {Public.NotificationTable} n ON a.{Public.Alerts.NotificationChannelId} = n.{Public.NotificationChannels.Id}
             JOIN
                 {Public.DataSourceTable} d ON a.{Public.Alerts.DataSourceId} = d.{Public.DataSources.Id}
             WHERE a.{Public.Alerts.Description} ILIKE @p_description
             LIMIT @limit OFFSET @offset
             """;
    }
}