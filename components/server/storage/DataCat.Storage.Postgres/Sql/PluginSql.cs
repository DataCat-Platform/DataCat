namespace DataCat.Storage.Postgres.Sql;

public static class PluginSql
{
    public static class Select
    {
        public const string SearchPluginsTotalCount = $"""
             SELECT
                 COUNT(*)
             FROM {Public.PluginTable}
             WHERE {Public.Plugins.Name} ILIKE @p_name
         """;
        
        public const string SearchPlugins = $"""
            SELECT
                {Public.Plugins.Id}               {nameof(PluginSnapshot.Id)},
                {Public.Plugins.Name}             {nameof(PluginSnapshot.Name)},
                {Public.Plugins.Version}          {nameof(PluginSnapshot.Version)},
                {Public.Plugins.Description}      {nameof(PluginSnapshot.Description)},
                {Public.Plugins.Author}           {nameof(PluginSnapshot.Author)},
                {Public.Plugins.IsEnabled}        {nameof(PluginSnapshot.IsEnabled)},
                {Public.Plugins.Settings}         {nameof(PluginSnapshot.Settings)},
                {Public.Plugins.CreatedAt}        {nameof(PluginSnapshot.CreatedAt)},
                {Public.Plugins.UpdatedAt}        {nameof(PluginSnapshot.UpdatedAt)}
            FROM {Public.PluginTable}
            WHERE {Public.Plugins.Name} ILIKE @p_name
            LIMIT @limit OFFSET @offset
        """;
    }
}