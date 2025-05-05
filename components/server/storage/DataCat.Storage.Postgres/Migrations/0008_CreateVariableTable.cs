namespace DataCat.Server.Postgres.Migrations;

[Migration(8)]
public sealed class CreateVariableTable : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;
    
    static CreateVariableTable()
    {
        UpSql = @$"
            CREATE TABLE {Public.VariableTable} (
                {Public.Variables.Id} TEXT PRIMARY KEY,
                {Public.Variables.Placeholder} TEXT NOT NULL,
                {Public.Variables.Value} TEXT NOT NULL,
                {Public.Variables.NamespaceId} TEXT NOT NULL,
                {Public.Variables.DashboardId} TEXT NULL,
                FOREIGN KEY ({Public.Variables.NamespaceId}) REFERENCES {Public.NamespaceTable}({Public.Namespaces.Id}) ON DELETE CASCADE,
                FOREIGN KEY ({Public.Variables.DashboardId}) REFERENCES {Public.DashboardTable}({Public.Dashboards.Id}) ON DELETE CASCADE,
                
                CONSTRAINT UQ_Variable_Placeholder_Scope UNIQUE NULLS NOT DISTINCT (
                    {Public.Variables.Placeholder},
                    {Public.Variables.NamespaceId},
                    {Public.Variables.DashboardId}
                )
            );
        ";
        
        DownSql = @$"
            DROP TABLE IF EXISTS {Public.VariableTable};
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