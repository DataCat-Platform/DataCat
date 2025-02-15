namespace DataCat.Server.Postgres.Migrations;

[Migration(2)]
public sealed class CreateUsersTable : Migration 
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreateUsersTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.UserTable} (
                {Public.Users.UserId} TEXT PRIMARY KEY,
                {Public.Users.UserName} TEXT NOT NULL,
                {Public.Users.UserEmail} TEXT NOT NULL,
                {Public.Users.UserRole} INT NOT NULL
            );
        ";
        DownSql = $"DROP TABLE IF EXISTS {Public.UserTable};";
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