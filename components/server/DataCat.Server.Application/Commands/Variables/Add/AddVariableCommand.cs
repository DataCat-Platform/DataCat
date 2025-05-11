namespace DataCat.Server.Application.Commands.Variables.Add;

public sealed record AddVariableCommand(
    string Placeholder,
    string Value,
    Guid NamespaceId,
    Guid? DashboardId = null) : ICommand<Guid>, IAuthorizedCommand;
