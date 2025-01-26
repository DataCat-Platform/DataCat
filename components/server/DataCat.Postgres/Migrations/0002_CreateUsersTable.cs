namespace DataCat.Server.Postgres.Migrations;

[Migration(2)]
public sealed class CreateUsersTable : Migration 
{
    public override void Up()
    {
        Execute.Sql(@$"
        CREATE TABLE {Public.UserTable} (
            {Public.Users.UserId} TEXT PRIMARY KEY,
            {Public.Users.Name} TEXT NOT NULL,
            {Public.Users.Email} TEXT NOT NULL,
            {Public.Users.Role} TEXT NOT NULL
        );
    ");
    }

    public override void Down()
    {
        Execute.Sql($"DROP TABLE IF EXISTS {Public.UserTable};");
    }
}