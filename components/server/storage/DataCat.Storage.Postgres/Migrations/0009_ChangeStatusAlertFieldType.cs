namespace DataCat.Server.Postgres.Migrations;

[Migration(9)]
public class ChangeStatusAlertFieldType : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static ChangeStatusAlertFieldType()
    {
        UpSql = @$"
            ALTER TABLE {Public.AlertTable}
            ALTER COLUMN {Public.Alerts.Status} TYPE TEXT USING {Public.Alerts.Status}::TEXT;

            UPDATE {Public.AlertTable}
            SET {Public.Alerts.Status} = 'Ok';

            ALTER TABLE {Public.AlertTable}
            ALTER COLUMN {Public.Alerts.Status} SET DEFAULT 'Ok';
        ";

        DownSql = @$"
            ALTER TABLE {Public.AlertTable}
            ALTER COLUMN {Public.Alerts.Status} DROP DEFAULT;

            UPDATE {Public.AlertTable}
            SET {Public.Alerts.Status} = '3';

            ALTER TABLE {Public.AlertTable}
            ALTER COLUMN {Public.Alerts.Status} TYPE INTEGER USING {Public.Alerts.Status}::INTEGER;
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