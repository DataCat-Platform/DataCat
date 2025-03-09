namespace DataCat.Server.Postgres.Migrations;

[Migration(3)]
public class CreateDataSourceTable : Migration 
{
    public static string UpSql = null!;
    public static string DownSql = null!;

    static CreateDataSourceTable()
    {
        UpSql = @$"
            --- crete data source table
            CREATE TABLE {Public.DataSourceTable} (
                {Public.DataSources.DataSourceId} TEXT PRIMARY KEY,
                {Public.DataSources.DataSourceName} TEXT NOT NULL,
                {Public.DataSources.DataSourceType} INT NOT NULL,
                {Public.DataSources.DataSourceConnectionString} TEXT NOT NULL
            );
        ";
        
        DownSql = @$"
            DROP TABLE IF EXISTS {Public.DataSourceTable};
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