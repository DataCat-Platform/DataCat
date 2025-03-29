namespace DataCat.Server.Application.Queries.Common.Responses;

public sealed record DataCatLayoutResponse
{
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
}

public static class DataCatLayoutResponseExtensions
{
    public static DataCatLayoutResponse ToResponse(this DataCatLayout layout)
    {
        return new DataCatLayoutResponse
        {
            X = layout.X,
            Y = layout.Y,
            Width = layout.Width,
            Height = layout.Height
        };
    }
}