namespace DataCat.Server.Application.Commands.Namespaces.Add;

public sealed record AddNamespaceCommand(string Name) : ICommand<Guid>; // todo: add admin request
