namespace DataCat.Server.Postgres.Migrations;

[Migration(5)]
public class CreateDashboardAndUserLink : Migration 
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreateDashboardAndUserLink()
    {
        UpSql = $@"
            --- add foreign key reference for DashboardOwnerId in DashboardTable
            ALTER TABLE {Public.DashboardTable}
            ADD CONSTRAINT FK_Dashboard_Owner 
                FOREIGN KEY ({Public.Dashboards.DashboardOwnerId}) REFERENCES {Public.UserTable}({Public.Users.UserId});

            --- create many-to-many relationship table between dashboards and users
            CREATE TABLE {Public.DashboardUserLinkTable} (
                {Public.Dashboards.DashboardId} TEXT NOT NULL,
                {Public.Users.UserId} TEXT NOT NULL,
                PRIMARY KEY ({Public.Dashboards.DashboardId}, {Public.Users.UserId}),
                FOREIGN KEY ({Public.Dashboards.DashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.DashboardId}),
                FOREIGN KEY ({Public.Users.UserId}) REFERENCES {Public.UserTable}({Public.Users.UserId})
            );
        ";
        
        DownSql = @$"
            DROP TABLE IF EXISTS {Public.DashboardUserLinkTable};
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