namespace DataCat.Server.Postgres.Migrations.Migrations;

[Migration(1)]
public sealed class СreatePluginTable : Migration 
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE plugin (
                plugin_id TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                version TEXT NOT NULL,
                description TEXT NULL,
                author TEXT NOT NULL,
                is_enabled BOOLEAN NOT NULL,
                settings TEXT NULL,
                created_at TIMESTAMP NOT NULL,
                updated_at TIMESTAMP NOT NULL,
                last_loaded_at TIMESTAMP NULL
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS plugin;");
    }
}