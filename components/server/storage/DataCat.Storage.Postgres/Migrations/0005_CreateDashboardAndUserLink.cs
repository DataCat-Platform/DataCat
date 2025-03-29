namespace DataCat.Server.Postgres.Migrations;

[Migration(5)]
public class CreateDashboardAndUserLink : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateDashboardAndUserLink()
    {
        UpSql = $@"
            --- add foreign key reference for DashboardOwnerId in DashboardTable
            ALTER TABLE {Public.DashboardTable}
            ADD CONSTRAINT FK_Dashboard_Owner 
                FOREIGN KEY ({Public.Dashboards.OwnerId}) REFERENCES {Public.UserTable}({Public.Users.Id});

            --- create many-to-many relationship table between dashboards and users
            CREATE TABLE {Public.DashboardUserLinkTable} (
                {Public.Dashboards.Id} TEXT NOT NULL,
                {Public.Users.Id} TEXT NOT NULL,
                PRIMARY KEY ({Public.Dashboards.Id}, {Public.Users.Id}),
                FOREIGN KEY ({Public.Dashboards.Id}) REFERENCES {Public.DashboardTable}({Public.Dashboards.Id}),
                FOREIGN KEY ({Public.Users.Id}) REFERENCES {Public.UserTable}({Public.Users.Id})
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