namespace DataCat.Server.Postgres.Migrations;

[Migration(2)]
public sealed class CreateUsersTable : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateUsersTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.UserTable} (
                {Public.Users.Id} TEXT PRIMARY KEY
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