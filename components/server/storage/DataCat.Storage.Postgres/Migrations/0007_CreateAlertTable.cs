namespace DataCat.Server.Postgres.Migrations;

[Migration(7)]
public class CreateAlertAndNotificationChannelTable : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateAlertAndNotificationChannelTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.NotificationTable} (
                {Public.NotificationChannels.Id} TEXT PRIMARY KEY,
                {Public.NotificationChannels.DestinationId} INT NOT NULL,
                {Public.NotificationChannels.Settings} TEXT NOT NULL
            );

            CREATE TABLE {Public.AlertTable} (
                {Public.Alerts.Id} TEXT PRIMARY KEY,
                {Public.Alerts.Description} TEXT NULL,
                {Public.Alerts.Status} INT NOT NULL,
                {Public.Alerts.RawQuery} TEXT NOT NULL,
                {Public.Alerts.DataSourceId} TEXT NOT NULL,
                {Public.Alerts.NotificationChannelId} TEXT NOT NULL,
                {Public.Alerts.PreviousExecution} TIMESTAMP NOT NULL,
                {Public.Alerts.NextExecution} TIMESTAMP NOT NULL,
                {Public.Alerts.WaitTimeBeforeAlertingInTicks} BIGINT NOT NULL,
                {Public.Alerts.RepeatIntervalInTicks} BIGINT NOT NULL,
                FOREIGN KEY ({Public.Alerts.DataSourceId}) REFERENCES {Public.DataSourceTable}({Public.DataSources.Id})
                    ON DELETE CASCADE,
                FOREIGN KEY ({Public.Alerts.NotificationChannelId}) REFERENCES {Public.NotificationTable}({Public.NotificationChannels.Id})
                    ON DELETE CASCADE
            );
        ";
        
        DownSql = @$"
            DROP TABLE IF EXISTS {Public.NotificationTable};
            DROP TABLE IF EXISTS {Public.AlertTable};
        ";
    }
    
    public override void Up()
    {
        Execute.Sql(UpSql);
    }

    public override void Down()
    {
        Execute.Sql(DownSql);
    }
}