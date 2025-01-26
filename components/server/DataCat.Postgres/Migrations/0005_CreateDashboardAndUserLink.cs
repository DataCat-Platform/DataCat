namespace DataCat.Server.Postgres.Migrations;

[Migration(5)]
public class CreateDashboardAndUserLink : Migration 
{
    public override void Up()
    {
        Execute.Sql($@"
            --- add foreign key reference for OwnerId in DashboardTable
            ALTER TABLE {Public.DashboardTable}
            ADD CONSTRAINT FK_Dashboard_Owner 
                FOREIGN KEY ({Public.Dashboards.OwnerId}) REFERENCES {Public.UserTable}({Public.Users.UserId});

            --- create many-to-many relationship table between dashboards and users
            CREATE TABLE {Public.DashboardUserLinkTable} (
                {Public.Dashboards.DashboardId} TEXT NOT NULL,
                {Public.Users.UserId} TEXT NOT NULL,
                PRIMARY KEY ({Public.Dashboards.DashboardId}, {Public.Users.UserId}),
                FOREIGN KEY ({Public.Dashboards.DashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.DashboardId}),
                FOREIGN KEY ({Public.Users.UserId}) REFERENCES {Public.UserTable}({Public.Users.UserId})
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@$"
            DROP TABLE IF EXISTS {Public.DashboardUserLinkTable};
        ");
    }
}