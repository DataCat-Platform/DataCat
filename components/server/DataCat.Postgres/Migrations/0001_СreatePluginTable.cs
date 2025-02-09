namespace DataCat.Server.Postgres.Migrations;

[Migration(1)]
public sealed class CreatePluginTable : Migration
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreatePluginTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.PluginTable} (
                {Public.Plugins.PluginId} TEXT PRIMARY KEY,
                {Public.Plugins.PluginName} TEXT NOT NULL,
                {Public.Plugins.PluginVersion} TEXT NOT NULL,
                {Public.Plugins.PluginDescription} TEXT NULL,
                {Public.Plugins.PluginAuthor} TEXT NOT NULL,
                {Public.Plugins.PluginIsEnabled} BOOLEAN NOT NULL,
                {Public.Plugins.PluginSettings} TEXT NULL,
                {Public.Plugins.PluginCreatedAt} TIMESTAMP NOT NULL,
                {Public.Plugins.PluginUpdatedAt} TIMESTAMP NOT NULL
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