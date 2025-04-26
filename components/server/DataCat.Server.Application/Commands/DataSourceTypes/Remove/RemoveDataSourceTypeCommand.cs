namespace DataCat.Server.Application.Commands.DataSourceTypes.Remove;

public sealed record RemoveDataSourceTypeCommand(string Name) : IRequest<Result>, IAdminRequest;