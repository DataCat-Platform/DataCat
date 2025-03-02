namespace DataCat.Server.Application.Commands.DataSource.Remove;

public sealed record RemoveDataSourceCommand(string DataSourceId) : IRequest<Result>, IAdminRequest;
