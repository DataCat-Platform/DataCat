namespace DataCat.Server.Application.Commands.DataSourceTypes.Add;

public sealed record AddDataSourceTypeCommand(string Name) : IRequest<Result<int>>, IAdminRequest;
