namespace DataCat.Server.Application.Commands.Variables.Remove;

public sealed record RemoveVariableCommand(Guid Id) : ICommand, IAuthorizedCommand;
