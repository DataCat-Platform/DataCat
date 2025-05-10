namespace DataCat.Server.Postgres.Migrations;

[Migration(10)]
public class AddNamespaceIdFieldToTables : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    private const string DefaultGuid = ApplicationConstants.DefaultNamespaceId;

    static AddNamespaceIdFieldToTables()
    {
        UpSql = @$"
            ALTER TABLE {Public.PanelTable}
            ADD COLUMN {Public.Panels.NamespaceId} TEXT NOT NULL DEFAULT {DefaultGuid};

            ALTER TABLE {Public.NotificationChannelTable}
            ADD COLUMN {Public.NotificationChannels.NamespaceId} TEXT NOT NULL DEFAULT {DefaultGuid};

            ALTER TABLE {Public.NotificationChannelGroupTable}
            ADD COLUMN {Public.NotificationChannelGroups.NamespaceId} TEXT NOT NULL DEFAULT {DefaultGuid};

            ALTER TABLE {Public.AlertTable}
            ADD COLUMN {Public.Alerts.NamespaceId} TEXT NOT NULL DEFAULT {DefaultGuid};
        ";

        DownSql = @$"
            ALTER TABLE {Public.PanelTable}
            DROP COLUMN {Public.Panels.NamespaceId};

            ALTER TABLE {Public.NotificationChannelTable}
            DROP COLUMN {Public.NotificationChannels.NamespaceId};

            ALTER TABLE {Public.NotificationChannelGroupTable}
            DROP COLUMN {Public.NotificationChannelGroups.NamespaceId};

            ALTER TABLE {Public.AlertTable}
            DROP COLUMN {Public.Alerts.NamespaceId};
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