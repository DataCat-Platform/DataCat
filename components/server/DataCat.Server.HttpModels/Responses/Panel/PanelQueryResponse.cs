namespace DataCat.Server.HttpModels.Responses.Panel;

public class PanelQueryResponse
{
    public required string Query { get; init; }

    public required DataSourceResponse DataSource { get; init; }
}