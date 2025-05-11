using DataCat.Storage.Postgres.Schemes;

namespace DataCat.Server.Postgres.Migrations;

[Migration(1)]
public sealed class CreatePluginTable : Migration
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreatePluginTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.PluginTable} (
                {Public.Plugins.Id} TEXT PRIMARY KEY,
                {Public.Plugins.Name} TEXT NOT NULL,
                {Public.Plugins.Version} TEXT NOT NULL,
                {Public.Plugins.Description} TEXT NULL,
                {Public.Plugins.Author} TEXT NOT NULL,
                {Public.Plugins.IsEnabled} BOOLEAN NOT NULL,
                {Public.Plugins.Settings} TEXT NULL,
                {Public.Plugins.CreatedAt} TIMESTAMP NOT NULL,
                {Public.Plugins.UpdatedAt} TIMESTAMP NOT NULL
            );
        ";
        DownSql = $"DROP TABLE IF EXISTS {Public.PluginTable};";
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