namespace DataCat.Server.Postgres.Migrations;

[Migration(8)]
public class AddTemplateToAlerts : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static AddTemplateToAlerts()
    {
        UpSql = @$"
            ALTER TABLE {Public.AlertTable}
            ADD COLUMN {Public.Alerts.Template} TEXT NULL;
        ";

        DownSql = @$"
            ALTER TABLE {Public.AlertTable}
            DROP COLUMN IF EXISTS {Public.Alerts.Template};
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