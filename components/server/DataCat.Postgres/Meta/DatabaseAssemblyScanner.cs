namespace DataCat.Server.Postgres.Meta;

public class DatabaseAssemblyScanner : IDatabaseAssemblyScanner
{
    private static List<(string MigrationName, dynamic UpSql, dynamic DownSql)>? cache = null;
    
    public List<(string MigrationName, dynamic UpSql, dynamic DownSql)> GetDatabaseSchema()
    {
        if (cache is not null)
            return cache;
        
        var migrationsAssembly = Assembly.GetAssembly(typeof(DatabaseAssemblyScanner));
        var migrationTypes = migrationsAssembly!.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Migration)) && !t.IsAbstract);

        var result = new List<(string, dynamic, dynamic)>();

        foreach (var type in migrationTypes)
        {
            var upSqlField = type.GetField("UpSql", BindingFlags.Public | BindingFlags.Static);
            var downSqlField = type.GetField("DownSql", BindingFlags.Public | BindingFlags.Static);

            var upSql = upSqlField?.GetValue(null)?.ToString() ?? "";
            var downSql = downSqlField?.GetValue(null)?.ToString() ?? "";

            var upSqlParsed = upSql.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x));
            var downSqlParsed = downSql.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x));
            result.Add((type.Name, upSqlParsed, downSqlParsed));
        }
        
        cache = result;

        return result;
    }
}