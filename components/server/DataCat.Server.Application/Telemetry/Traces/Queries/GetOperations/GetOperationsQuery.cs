namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetOperations;

public sealed record GetOperationsQuery(
    string DataSourceName,
    string ServiceName) : IRequest<Result<IEnumerable<string>>>, IAuthorizedQuery;