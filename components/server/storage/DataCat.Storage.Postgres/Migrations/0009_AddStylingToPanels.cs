namespace DataCat.Server.Postgres.Migrations;

[Migration(9)]
public sealed class AddStylingToPanels : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static AddStylingToPanels()
    {
        UpSql = @$"
            ALTER TABLE {Public.PanelTable}
            ADD COLUMN {Public.Panels.StylingConfiguration} TEXT NULL;
        ";

        DownSql = @$"
            ALTER TABLE {Public.PanelTable}
            DROP COLUMN IF EXISTS {Public.Panels.StylingConfiguration};
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