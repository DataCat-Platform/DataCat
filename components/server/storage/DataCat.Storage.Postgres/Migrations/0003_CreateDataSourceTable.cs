namespace DataCat.Server.Postgres.Migrations;

[Migration(3)]
public class CreateDataSourceTable : Migration 
{
    public static readonly string UpSql = null!;
    public static readonly string DownSql = null!;

    static CreateDataSourceTable()
    {
        UpSql = @$"
            --- crete data source table
            CREATE TABLE {Public.DataSourceTable} (
                {Public.DataSources.Id} TEXT PRIMARY KEY,
                {Public.DataSources.Name} TEXT NOT NULL,
                {Public.DataSources.TypeId} INT NOT NULL,
                {Public.DataSources.ConnectionString} TEXT NOT NULL
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