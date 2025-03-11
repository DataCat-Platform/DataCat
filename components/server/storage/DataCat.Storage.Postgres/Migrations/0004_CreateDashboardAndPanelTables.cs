namespace DataCat.Server.Postgres.Migrations;

[Migration(4)]
public class CreateDashboardAndPanelTables : Migration 
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreateDashboardAndPanelTables()
    {
        UpSql = @$"
            --- create dashboard table
            CREATE TABLE {Public.DashboardTable} (
                {Public.Dashboards.DashboardId} TEXT PRIMARY KEY,
                {Public.Dashboards.DashboardName} TEXT NOT NULL,
                {Public.Dashboards.DashboardDescription} TEXT NOT NULL,
                {Public.Dashboards.DashboardOwnerId} TEXT NOT NULL,
                {Public.Dashboards.DashboardCreatedAt} TIMESTAMP NOT NULL,
                {Public.Dashboards.DashboardUpdatedAt} TIMESTAMP NOT NULL
            );

            --- create panel table
            CREATE TABLE {Public.PanelTable} (
                {Public.Panels.PanelId} TEXT PRIMARY KEY,
                {Public.Panels.PanelTitle} TEXT NOT NULL,
                {Public.Panels.PanelType} INT NOT NULL,
                {Public.Panels.PanelRawQuery} TEXT NOT NULL,
                {Public.Panels.PanelDataSourceId} TEXT NOT NULL,
                {Public.Panels.PanelX} INT NOT NULL,
                {Public.Panels.PanelY} INT NOT NULL,
                {Public.Panels.PanelWidth} INT NOT NULL,
                {Public.Panels.PanelHeight} INT NOT NULL,
                {Public.Panels.PanelParentDashboardId} TEXT NOT NULL,
                FOREIGN KEY ({Public.Panels.PanelParentDashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.DashboardId}) 
                    ON DELETE CASCADE,
                FOREIGN KEY ({Public.Panels.PanelDataSourceId}) REFERENCES {Public.DataSourceTable}({Public.DataSources.DataSourceId})
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