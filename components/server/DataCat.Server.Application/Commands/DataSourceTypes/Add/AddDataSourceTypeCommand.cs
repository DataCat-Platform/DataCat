namespace DataCat.Server.Application.Commands.DataSourceTypes.Add;

public sealed record AddDataSourceTypeCommand(string Name) : ICommand<int>, IAdminRequest;
