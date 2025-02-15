namespace DataCat.Server.Application.Assembly;

public interface IDatabaseAssemblyScanner
{
    List<(string MigrationName, dynamic UpSql, dynamic DownSql)> GetDatabaseSchema();
    
    List<string[]> GetSqlQueries();
}