namespace DataCat.Server.Postgres.Migrations;

[Migration(4)]
public class CreateDashboardAndPanelTables : Migration 
{
    public override void Up()
    {
        Execute.Sql(@$"
        --- create panel types table
        CREATE TABLE {Public.PanelTypeTable} (
            {Public.PanelTypes.Id} INT PRIMARY KEY,
            {Public.PanelTypes.Type} TEXT NOT NULL
        );

        --- create dashboard table
        CREATE TABLE {Public.DashboardTable} (
            {Public.Dashboards.DashboardId} TEXT PRIMARY KEY,
            {Public.Dashboards.Name} TEXT NOT NULL,
            {Public.Dashboards.Description} TEXT NOT NULL,
            {Public.Dashboards.OwnerId} TEXT NOT NULL,
            {Public.Dashboards.CreatedAt} TIMESTAMP NOT NULL,
            {Public.Dashboards.UpdatedAt} TIMESTAMP NOT NULL
        );

        --- create panel table
        CREATE TABLE {Public.PanelTable} (
            {Public.Panels.PanelId} TEXT PRIMARY KEY,
            {Public.Panels.Title} TEXT NOT NULL,
            {Public.Panels.PanelType} INT NOT NULL,
            {Public.Panels.RawQuery} TEXT NOT NULL,
            {Public.Panels.DataSource} TEXT NOT NULL,
            {Public.Panels.X} INT NOT NULL,
            {Public.Panels.Y} INT NOT NULL,
            {Public.Panels.Width} INT NOT NULL,
            {Public.Panels.Height} INT NOT NULL,
            {Public.Panels.ParentDashboardId} TEXT NOT NULL,
            FOREIGN KEY ({Public.Panels.PanelType}) REFERENCES {Public.PanelTypeTable}({Public.PanelTypes.Id}),
            FOREIGN KEY ({Public.Panels.ParentDashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.DashboardId}),
            FOREIGN KEY ({Public.Panels.DataSource}) REFERENCES {Public.DataSourceTable}({Public.DataSources.DataSourceId})
        );
    ");
    }

    public override void Down()
    {
        Execute.Sql(@$"
            DROP TABLE IF EXISTS {Public.PanelTable};
            DROP TABLE IF EXISTS {Public.PanelTypeTable};
            DROP TABLE IF EXISTS {Public.DashboardTable};
        ");
    }
}