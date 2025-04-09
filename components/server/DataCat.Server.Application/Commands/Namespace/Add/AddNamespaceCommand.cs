namespace DataCat.Server.Application.Commands.Namespace.Add;

public sealed record AddNamespaceCommand(string Name) : IRequest<Result<Guid>>;
