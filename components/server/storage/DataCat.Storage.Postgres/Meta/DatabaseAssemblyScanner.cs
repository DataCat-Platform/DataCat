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
        var sqlQueries = new List<string[]>();

        sqlQueries.AddRange(GetSqlQueriesInternal(typeof(AlertSql)));
        sqlQueries.AddRange(GetSqlQueriesInternal(typeof(DashboardSql)));

        return sqlQueries;
    }

    private static List<string[]> GetSqlQueriesInternal(Type type)
    {
        var values = new List<string[]>();
        foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
        {
            var constants = nestedType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f is { IsLiteral: true, IsInitOnly: false })
                .Select(f => f.GetValue(null)?.ToString());
            values.AddRange(constants.Select(x => x?.Split("\n")).ToList()!);
        }
        return values;
    }
}