namespace DataCat.Server.Application.Commands.Variables.Update;

public sealed record UpdateVariableCommand(
    Guid Id,
    string Placeholder,
    string Value) : ICommand, IAuthorizedCommand;
