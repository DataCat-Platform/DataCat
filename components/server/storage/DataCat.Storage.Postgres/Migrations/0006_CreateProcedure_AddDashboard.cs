namespace DataCat.Server.Postgres.Migrations;

[Migration(6)]
public class CreateProcedure_AddDashboard : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateProcedure_AddDashboard()
    {
        UpSql = @$"
            CREATE OR REPLACE PROCEDURE {DashboardProcedures.AddDashboard}(
                p_dashboard_id TEXT,
                p_dashboard_name TEXT,
                p_dashboard_description TEXT,
                p_dashboard_owner_id TEXT
            ) AS $$
            BEGIN
                INSERT INTO {Public.DashboardTable} (
                    {Public.Dashboards.Id},
                    {Public.Dashboards.Name},
                    {Public.Dashboards.Description},
                    {Public.Dashboards.OwnerId},
                    {Public.Dashboards.CreatedAt},
                    {Public.Dashboards.UpdatedAt}
                ) VALUES (
                    p_dashboard_id,
                    p_dashboard_name,
                    p_dashboard_description,
                    p_dashboard_owner_id,
                    NOW(),
                    NOW()
                );

                INSERT INTO {Public.DashboardUserLinkTable} (
                    {Public.Dashboards.Id},
                    {Public.Users.Id}
                ) VALUES (
                    p_dashboard_id,
                    p_dashboard_owner_id
                );
            END;
            $$ LANGUAGE plpgsql;
        ";
        
        DownSql = @$"DROP PROCEDURE IF EXISTS {DashboardProcedures.AddDashboard}(TEXT, TEXT, TEXT, TEXT);";
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