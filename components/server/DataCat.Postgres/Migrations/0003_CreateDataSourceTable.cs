namespace DataCat.Server.Postgres.Migrations;

[Migration(3)]
public class CreateDataSourceTable : Migration 
{
    public override void Up()
    {
        Execute.Sql(@$"
            --- create data source type table
            CREATE TABLE {Public.DataSourceTypeTable} (
                {Public.DataSourceTypes.Id} INT PRIMARY KEY,
                {Public.DataSourceTypes.Source} TEXT NOT NULL
            );

            --- crete data source table
            CREATE TABLE {Public.DataSourceTable} (
                {Public.DataSources.DataSourceId} TEXT PRIMARY KEY,
                {Public.DataSources.Name} TEXT NOT NULL,
                {Public.DataSources.DataSourceType} INT NOT NULL,
                {Public.DataSources.ConnectionString} TEXT NOT NULL,
                FOREIGN KEY ({Public.DataSources.DataSourceType}) REFERENCES {Public.DataSourceTypeTable}({Public.DataSourceTypes.Id})
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@$"
            DROP TABLE IF EXISTS {Public.DataSourceTable};
            DROP TABLE IF EXISTS {Public.DataSourceTypeTable};
        ");
    }
}