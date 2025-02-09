namespace DataCat.Server.Application.Commands.Dashboard.Update;

public sealed record UpdateDashboardCommand : IRequest<Result>
{
    public required string DataSourceId { get; init; }

    public required string ConnectionString { get; init; }
}