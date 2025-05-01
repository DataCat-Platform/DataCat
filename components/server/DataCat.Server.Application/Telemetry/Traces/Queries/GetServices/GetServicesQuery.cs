namespace DataCat.Server.Application.Telemetry.Traces.Queries.GetServices;

public sealed record GetServicesQuery(
    string DataSourceName) : IRequest<Result<IEnumerable<string>>>, IAuthorizedQuery;