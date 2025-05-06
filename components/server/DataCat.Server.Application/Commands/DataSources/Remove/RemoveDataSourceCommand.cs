namespace DataCat.Server.Application.Commands.DataSources.Remove;

public sealed record RemoveDataSourceCommand(Guid Id) : ICommand, IAdminRequest;
