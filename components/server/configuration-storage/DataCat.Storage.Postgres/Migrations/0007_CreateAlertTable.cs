namespace DataCat.Server.Postgres.Migrations;

[Migration(7)]
public class CreateAlertAndNotificationChannelTable : Migration 
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreateAlertAndNotificationChannelTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.NotificationTable} (
                {Public.NotificationChannels.NotificationChannelId} TEXT PRIMARY KEY,
                {Public.NotificationChannels.NotificationDestination} INT NOT NULL,
                {Public.NotificationChannels.NotificationSettings} TEXT NOT NULL
            );

            CREATE TABLE {Public.AlertTable} (
                {Public.Alerts.AlertId} TEXT PRIMARY KEY,
                {Public.Alerts.AlertDescription} TEXT NOT NULL,
                {Public.Alerts.AlertStatus} INT NOT NULL,
                {Public.Alerts.AlertRawQuery} TEXT NOT NULL,
                {Public.Alerts.AlertDataSourceId} TEXT NOT NULL,
                {Public.Alerts.AlertNotificationChannelId} TEXT NOT NULL,
                FOREIGN KEY ({Public.Alerts.AlertDataSourceId}) REFERENCES {Public.DataSourceTable}({Public.DataSources.DataSourceId})
                    ON DELETE CASCADE,
                FOREIGN KEY ({Public.Alerts.AlertNotificationChannelId}) REFERENCES {Public.NotificationTable}({Public.NotificationChannels.NotificationChannelId})
                    ON DELETE CASCADE
            );
        ";
        
        DownSql = @$"
            DROP FUNCTION IF EXISTS {Public.NotificationTable};
            DROP FUNCTION IF EXISTS {Public.AlertTable};
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