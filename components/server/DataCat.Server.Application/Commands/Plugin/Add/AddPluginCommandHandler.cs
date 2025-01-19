namespace DataCat.Server.Application.Commands.Plugin.Add;

public sealed record AddPluginCommandHandler : IRequestHandler<AddPluginCommand, Result>
{
    public AddPluginCommandHandler()
    {
        
    }
    
    public Task<Result> Handle(AddPluginCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("AddPluginCommandHandler");
        return Task.FromResult(Result.Success());
    }
}