namespace DataCat.Server.Application.Commands.DataSources.Remove;

public sealed record RemoveDataSourceCommand(string DataSourceName) : ICommand, IAdminRequest;
