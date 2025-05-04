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
       """;
        
        public const string SearchDataSources = $"""
             SELECT
                  data_source.{Public.DataSources.Id}                    {nameof(DataSourceSnapshot.Id)},
                  data_source.{Public.DataSources.Name}                  {nameof(DataSourceSnapshot.Name)},
                  data_source.{Public.DataSources.TypeId}                {nameof(DataSourceSnapshot.TypeId)},
                  data_source.{Public.DataSources.ConnectionSettings}    {nameof(DataSourceSnapshot.ConnectionSettings)},
                  data_source.{Public.DataSources.Purpose}               {nameof(DataSourceSnapshot.Purpose)},
             
                  data_source_type.{Public.DataSourceType.Id}          {nameof(DataSourceTypeSnapshot.Id)},
                  data_source_type.{Public.DataSourceType.Name}        {nameof(DataSourceTypeSnapshot.Name)}
             
             FROM {Public.DataSourceTable} data_source
             JOIN {Public.DataSourceTypeTable} data_source_type ON data_source.{Public.DataSources.TypeId} = data_source_type.{Public.DataSourceType.Id}
             WHERE data_source.{Public.DataSources.Name} ILIKE @p_name
             LIMIT @limit OFFSET @offset
         """;
        
        public const string GetByName = $"""
             SELECT
                  data_source.{Public.DataSources.Id}                    {nameof(DataSourceSnapshot.Id)},
                  data_source.{Public.DataSources.Name}                  {nameof(DataSourceSnapshot.Name)},
                  data_source.{Public.DataSources.TypeId}                {nameof(DataSourceSnapshot.TypeId)},
                  data_source.{Public.DataSources.ConnectionSettings}    {nameof(DataSourceSnapshot.ConnectionSettings)},
                  data_source.{Public.DataSources.Purpose}               {nameof(DataSourceSnapshot.Purpose)},
             
                  data_source_type.{Public.DataSourceType.Id}          {nameof(DataSourceTypeSnapshot.Id)},
                  data_source_type.{Public.DataSourceType.Name}        {nameof(DataSourceTypeSnapshot.Name)}
             
             FROM {Public.DataSourceTable} data_source
             JOIN {Public.DataSourceTypeTable} data_source_type ON data_source.{Public.DataSources.TypeId} = data_source_type.{Public.DataSourceType.Id}
             WHERE data_source.{Public.DataSources.Name} ILIKE @p_name
         """;
        
        public const string GetAll = $"""
            SELECT
                 data_source.{Public.DataSources.Id}                    {nameof(DataSourceSnapshot.Id)},
                 data_source.{Public.DataSources.Name}                  {nameof(DataSourceSnapshot.Name)},
                 data_source.{Public.DataSources.TypeId}                {nameof(DataSourceSnapshot.TypeId)},
                 data_source.{Public.DataSources.ConnectionSettings}    {nameof(DataSourceSnapshot.ConnectionSettings)},
                 data_source.{Public.DataSources.Purpose}               {nameof(DataSourceSnapshot.Purpose)},
            
                 data_source_type.{Public.DataSourceType.Id}          {nameof(DataSourceTypeSnapshot.Id)},
                 data_source_type.{Public.DataSourceType.Name}        {nameof(DataSourceTypeSnapshot.Name)}
            
            FROM {Public.DataSourceTable} data_source
            JOIN {Public.DataSourceTypeTable} data_source_type ON data_source.{Public.DataSources.TypeId} = data_source_type.{Public.DataSourceType.Id};
        """;
    }
}