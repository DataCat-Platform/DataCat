namespace DataCat.Storage.Postgres.Sql;

public static class AlertSql
{
    public static class Select
    {
        public const string GetById = $@"
             SELECT
                alert.{Public.Alerts.Id}                                {nameof(AlertSnapshot.Id)},
                alert.{Public.Alerts.Description}                       {nameof(AlertSnapshot.Description)},
                alert.{Public.Alerts.Template}                          {nameof(AlertSnapshot.Template)},
                alert.{Public.Alerts.Status}                            {nameof(AlertSnapshot.Status)},
                alert.{Public.Alerts.RawQuery}                          {nameof(AlertSnapshot.ConditionQuery)},
                alert.{Public.Alerts.DataSourceId}                      {nameof(AlertSnapshot.DataSourceId)},
                alert.{Public.Alerts.NotificationChannelGroupId}        {nameof(AlertSnapshot.NotificationChannelGroupId)},
                alert.{Public.Alerts.PreviousExecution}                 {nameof(AlertSnapshot.PreviousExecution)},
                alert.{Public.Alerts.NextExecution}                     {nameof(AlertSnapshot.NextExecution)},
                alert.{Public.Alerts.WaitTimeBeforeAlertingInTicks}     {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                alert.{Public.Alerts.RepeatIntervalInTicks}             {nameof(AlertSnapshot.RepeatIntervalInTicks)},
                alert.{Public.Alerts.Tags}                              {nameof(AlertSnapshot.Tags)},
                alert.{Public.Alerts.NamespaceId}                       {nameof(AlertSnapshot.NamespaceId)},
                
                notification_group.{Public.NotificationChannelGroups.Id}                  {nameof(NotificationChannelGroupSnapshot.Id)},
                notification_group.{Public.NotificationChannelGroups.Name}                {nameof(NotificationChannelGroupSnapshot.Name)},
                notification_group.{Public.NotificationChannelGroups.NamespaceId}         {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                
                notification_channel.{Public.NotificationChannels.Id}                            {nameof(NotificationChannelSnapshot.Id)},
                notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}    {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                notification_channel.{Public.NotificationChannels.DestinationId}                 {nameof(NotificationChannelSnapshot.DestinationId)},
                notification_channel.{Public.NotificationChannels.Settings}                      {nameof(NotificationChannelSnapshot.Settings)},
                notification_channel.{Public.NotificationChannels.NamespaceId}                   {nameof(NotificationChannelSnapshot.NamespaceId)},
                
                notification_destination.{Public.NotificationDestination.Id}                  {nameof(NotificationDestinationSnapshot.Id)},
                notification_destination.{Public.NotificationDestination.Name}                {nameof(NotificationDestinationSnapshot.Name)},
                
                data_source.{Public.DataSources.Id}                            {nameof(DataSourceSnapshot.Id)},
                data_source.{Public.DataSources.Name}                          {nameof(DataSourceSnapshot.Name)},
                data_source.{Public.DataSources.TypeId}                        {nameof(DataSourceSnapshot.TypeId)},
                data_source.{Public.DataSources.ConnectionSettings}            {nameof(DataSourceSnapshot.ConnectionSettings)},
                data_source.{Public.DataSources.Purpose}                       {nameof(DataSourceSnapshot.Purpose)},
                
                data_source_type.{Public.DataSourceType.Id}                        {nameof(DataSourceTypeSnapshot.Id)},
                data_source_type.{Public.DataSourceType.Name}                      {nameof(DataSourceTypeSnapshot.Name)}

             FROM 
                 {Public.AlertTable} alert
             JOIN
                 {Public.NotificationChannelGroupTable} notification_group ON alert.{Public.Alerts.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
             LEFT JOIN
                 {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
            LEFT JOIN
                 {Public.NotificationDestinationTable} notification_destination ON notification_destination.{Public.NotificationDestination.Id} = notification_channel.{Public.NotificationChannels.DestinationId}
             JOIN
                 {Public.DataSourceTable} data_source ON alert.{Public.Alerts.DataSourceId} = data_source.{Public.DataSources.Id}
             JOIN
                 {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId}
             WHERE alert.{Public.Alerts.Id} = @p_alert_id AND alert.{Public.Alerts.NamespaceId} = @p_namespace_id
             ";
        
        public const string SearchAlertsTotalCount =
            $"""
             SELECT
                COUNT(*)
             FROM 
                 {Public.AlertTable} alerts
             WHERE alerts.{Public.Alerts.NamespaceId} = @p_namespace_id 
             """;
        
        public const string SearchAlerts =
            $"""
             SELECT
                 alerts.{Public.Alerts.Id}                                       {nameof(AlertSnapshot.Id)},
                 alerts.{Public.Alerts.Description}                              {nameof(AlertSnapshot.Description)},
                 alerts.{Public.Alerts.Template}                                 {nameof(AlertSnapshot.Template)},
                 alerts.{Public.Alerts.Status}                                   {nameof(AlertSnapshot.Status)},
                 alerts.{Public.Alerts.RawQuery}                                 {nameof(AlertSnapshot.ConditionQuery)},
                 alerts.{Public.Alerts.DataSourceId}                             {nameof(AlertSnapshot.DataSourceId)},
                 alerts.{Public.Alerts.NotificationChannelGroupId}               {nameof(AlertSnapshot.NotificationChannelGroupId)},
                 alerts.{Public.Alerts.PreviousExecution}                        {nameof(AlertSnapshot.PreviousExecution)},
                 alerts.{Public.Alerts.NextExecution}                            {nameof(AlertSnapshot.NextExecution)},
                 alerts.{Public.Alerts.WaitTimeBeforeAlertingInTicks}            {nameof(AlertSnapshot.WaitTimeBeforeAlertingInTicks)},
                 alerts.{Public.Alerts.RepeatIntervalInTicks}                    {nameof(AlertSnapshot.RepeatIntervalInTicks)},
                 alerts.{Public.Alerts.Tags}                                     {nameof(AlertSnapshot.Tags)},
                 alerts.{Public.Alerts.NamespaceId}                              {nameof(AlertSnapshot.NamespaceId)},
                 
                 notification_group.{Public.NotificationChannelGroups.Id}             {nameof(NotificationChannelGroupSnapshot.Id)},
                 notification_group.{Public.NotificationChannelGroups.Name}           {nameof(NotificationChannelGroupSnapshot.Name)},
                 notification_group.{Public.NotificationChannelGroups.NamespaceId}    {nameof(NotificationChannelGroupSnapshot.NamespaceId)},
                 
                 notification_channel.{Public.NotificationChannels.Id}                           {nameof(NotificationChannelSnapshot.Id)},
                 notification_channel.{Public.NotificationChannels.NotificationChannelGroupId}   {nameof(NotificationChannelSnapshot.NotificationChannelGroupId)},
                 notification_channel.{Public.NotificationChannels.DestinationId}                {nameof(NotificationChannelSnapshot.DestinationId)},
                 notification_channel.{Public.NotificationChannels.Settings}                     {nameof(NotificationChannelSnapshot.Settings)},
                 notification_channel.{Public.NotificationChannels.NamespaceId}                  {nameof(NotificationChannelSnapshot.NamespaceId)},
                 
                 notification_destination.{Public.NotificationDestination.Id}        {nameof(NotificationDestinationSnapshot.Id)},
                 notification_destination.{Public.NotificationDestination.Name}      {nameof(NotificationDestinationSnapshot.Name)},
                 
                 data_source.{Public.DataSources.Id}                                 {nameof(DataSourceSnapshot.Id)},
                 data_source.{Public.DataSources.Name}                               {nameof(DataSourceSnapshot.Name)},
                 data_source.{Public.DataSources.TypeId}                             {nameof(DataSourceSnapshot.TypeId)},
                 data_source.{Public.DataSources.ConnectionSettings}                 {nameof(DataSourceSnapshot.ConnectionSettings)},
                 data_source.{Public.DataSources.Purpose}                            {nameof(DataSourceSnapshot.Purpose)},
                 
                 data_source_type.{Public.DataSourceType.Id}        {nameof(DataSourceTypeSnapshot.Id)},
                 data_source_type.{Public.DataSourceType.Name}      {nameof(DataSourceTypeSnapshot.Name)}
             
             FROM 
                 {Public.AlertTable} alerts
             JOIN
                 {Public.NotificationChannelGroupTable} notification_group ON alerts.{Public.Alerts.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
             LEFT JOIN
                 {Public.NotificationChannelTable} notification_channel ON notification_channel.{Public.NotificationChannels.NotificationChannelGroupId} = notification_group.{Public.NotificationChannelGroups.Id}
             LEFT JOIN
                {Public.NotificationDestinationTable} notification_destination ON notification_destination.{Public.NotificationDestination.Id} = notification_channel.{Public.NotificationChannels.DestinationId}
             JOIN
                 {Public.DataSourceTable} data_source ON alerts.{Public.Alerts.DataSourceId} = data_source.{Public.DataSources.Id}
             JOIN
                 {Public.DataSourceTypeTable} data_source_type ON data_source_type.{Public.DataSourceType.Id} = data_source.{Public.DataSources.TypeId}
             WHERE alerts.{Public.Alerts.NamespaceId} = @p_namespace_id 
             """;
    }
}