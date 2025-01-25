namespace DataCat.Server.Postgres.Migrations;

[Migration(1)]
public sealed class CreatePluginTable : Migration 
{
    public override void Up()
    {
        Execute.Sql(@$"
            CREATE TABLE {Public.PluginTable} (
                {Public.Plugins.PluginId} TEXT PRIMARY KEY,
                {Public.Plugins.Name} TEXT NOT NULL,
                {Public.Plugins.Version} TEXT NOT NULL,
                {Public.Plugins.Description} TEXT NULL,
                {Public.Plugins.Author} TEXT NOT NULL,
                {Public.Plugins.IsEnabled} BOOLEAN NOT NULL,
                {Public.Plugins.Settings} TEXT NULL,
                {Public.Plugins.CreatedAt} TIMESTAMP NOT NULL,
                {Public.Plugins.UpdatedAt} TIMESTAMP NOT NULL
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS plugin;");
    }
}