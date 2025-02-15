namespace DataCat.Server.HttpModels.Responses.Meta;

public class SchemaResponse
{
    public required string Migration { get; init; }

    public required dynamic UpSql { get; init; }

    public required dynamic DownSql { get; init; }
}