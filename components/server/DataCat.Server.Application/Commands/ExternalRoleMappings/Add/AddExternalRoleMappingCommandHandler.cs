namespace DataCat.Server.Application.Commands.ExternalRoleMappings.Add;

public sealed class AddExternalRoleMappingCommandHandler(
    IExternalRoleMappingRepository externalRoleMappingRepository,
    INamespaceService namespaceService)
    : IRequestHandler<AddExternalRoleMappingCommand, Result>
{
    public async Task<Result> Handle(AddExternalRoleMappingCommand request, CancellationToken cancellationToken)
    {
        var userRole = UserRole.FromValue(request.RoleId);

        var namespaceResult = await namespaceService.GetSpecificNamespaceOrDefaultAsync(request.NamespaceId, cancellationToken);
        if (namespaceResult.IsFailure)
        {
            return Result.Fail(namespaceResult.Errors!);
        }
        
        var externalRoleMappingResult = new ExternalRoleMappingValue(
            request.ExternalRole,
            userRole,
            namespaceResult.Value.Id);
        
        await externalRoleMappingRepository.AddExternalRoleMappingAsync(externalRoleMappingResult, cancellationToken); 
        
        return Result.Success();
    }
}