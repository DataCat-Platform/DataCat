namespace DataCat.Server.Postgres.Migrations;

[Migration(4)]
public class CreateDashboardAndPanelTables : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateDashboardAndPanelTables()
    {
        UpSql = @$"
            --- create dashboard table
            CREATE TABLE {Public.DashboardTable} (
                {Public.Dashboards.Id} TEXT PRIMARY KEY,
                {Public.Dashboards.Name} TEXT NOT NULL,
                {Public.Dashboards.Description} TEXT NOT NULL,
                {Public.Dashboards.NamespaceId} TEXT NOT NULL,
                {Public.Dashboards.CreatedAt} TIMESTAMP NOT NULL,
                {Public.Dashboards.UpdatedAt} TIMESTAMP NOT NULL,
                {Public.Dashboards.Tags} TEXT[] NOT NULL,
                FOREIGN KEY ({Public.Dashboards.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE
            );

            --- create panel table
            CREATE TABLE {Public.PanelTable} (
                {Public.Panels.Id} TEXT PRIMARY KEY,
                {Public.Panels.Title} TEXT NOT NULL,
                {Public.Panels.TypeId} INT NOT NULL,
                {Public.Panels.RawQuery} TEXT NOT NULL,
                {Public.Panels.DataSourceId} TEXT NOT NULL,
                {Public.Panels.LayoutConfiguration} TEXT NULL,
                {Public.Panels.DashboardId} TEXT NOT NULL,
                FOREIGN KEY ({Public.Panels.DashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.Id}) 
                    ON DELETE CASCADE,
                FOREIGN KEY ({Public.Panels.DataSourceId}) REFERENCES {Public.DataSourceTable}({Public.DataSources.Id})
            );
        ";
        
        DownSql = @$"
            DROP TABLE IF EXISTS {Public.PanelTable};
            DROP TABLE IF EXISTS {Public.DashboardTable};
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