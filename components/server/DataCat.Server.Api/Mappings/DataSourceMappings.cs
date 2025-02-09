namespace DataCat.Server.Api.Mappings;

public static class DataSourceMappings
{
    public static AddDataSourceCommand ToAddCommand(this AddDataSource request)
    {
        return new AddDataSourceCommand
        {
            Name = request.Name,
            ConnectionString = request.ConnectionString,
            Type = request.Type
        };
    }
    
    public static UpdateDataSourceCommand ToUpdateCommand(this UpdateDataSource request, string dataSourceId)
    {
        return new UpdateDataSourceCommand
        {
            DataSourceId = dataSourceId,
            ConnectionString = request.ConnectionString,
        };
    }

    public static DataSourceResponse ToResponse(this DataSourceEntity dataSource)
    {
        return new DataSourceResponse()
        {
            Id = dataSource.Id.ToString(),
            Name = dataSource.Name,
            ConnectionString = dataSource.ConnectionString,
            Type = dataSource.DataSourceType.Name
        };
    }
}