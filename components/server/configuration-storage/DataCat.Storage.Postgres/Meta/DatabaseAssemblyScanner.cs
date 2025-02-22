namespace DataCat.Storage.Postgres.Meta;

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
            var upSqlField = type.GetField(nameof(CreatePluginTable.UpSql), BindingFlags.Public | BindingFlags.Static);
            var downSqlField = type.GetField(nameof(CreatePluginTable.DownSql), BindingFlags.Public | BindingFlags.Static);

            var upSql = upSqlField?.GetValue(null)?.ToString() ?? "";
            var downSql = downSqlField?.GetValue(null)?.ToString() ?? "";

            var upSqlParsed = upSql.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x));
            var downSqlParsed = downSql.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x));
            result.Add((type.Name, upSqlParsed, downSqlParsed));
        }
        
        cache = result;

        return result;
    }

    public List<string[]> GetSqlQueries()
    {
        var fields = typeof(Sql)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => f.GetValue(null)?.ToString() ?? string.Empty)
            .ToList();

        var sqlQueries = fields.Select(x => x.Split("\n")).ToList();

        return sqlQueries;
    }
}