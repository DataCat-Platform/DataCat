namespace DataCat.Storage.Postgres.Sql;

public static class DataSourceSql
{
    public static class Select
    {
        public const string SearchDataSourcesTotalCount = $"""
           SELECT
                COUNT(*)
           FROM {Public.DataSourceTable}
           WHERE {Public.DataSources.Name} ILIKE @p_name
           LIMIT @limit OFFSET @offset
       """;
        
        public const string SearchDataSources = $"""
             SELECT
                  {Public.DataSources.Id}              {nameof(DataSourceSnapshot.Id)},
                  {Public.DataSources.Name}            {nameof(DataSourceSnapshot.Name)},
                  {Public.DataSources.TypeId}          {nameof(DataSourceSnapshot.TypeId)},
                  {Public.DataSources.ConnectionString}{nameof(DataSourceSnapshot.ConnectionString)}
             FROM {Public.DataSourceTable}
             WHERE {Public.DataSources.Name} ILIKE @p_name
             LIMIT @limit OFFSET @offset
         """;
    }
}