namespace DataCat.Server.Domain.Models.Enums;

public abstract class DataSourceType(string name, int value)
    : SmartEnum<DataSourceType, int>(name, value)
{
    public static readonly DataSourceType DataCat = new DataCatDataSource();
    public static readonly DataSourceType PostgreSQL = new PostgreSQLDataSource();
    public static readonly DataSourceType SQLLite = new SQLLiteDataSource();

    private sealed class DataCatDataSource() : DataSourceType("DataCat", 1);
    
    private sealed class PostgreSQLDataSource() : DataSourceType("PostgreSQL", 2);
    
    private sealed class SQLLiteDataSource() : DataSourceType("SQLLite", 3);
}
