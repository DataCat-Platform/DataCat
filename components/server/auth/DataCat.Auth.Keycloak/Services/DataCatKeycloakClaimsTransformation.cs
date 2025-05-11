namespace DataCat.Auth.Keycloak.Services;

public sealed class DataCatKeycloakClaimsTransformation(
    IServiceProvider serviceProvider,
    IIdentity identity) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true } ||
            principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        var roles = GetRolesFromPrincipal(principal);
        
        using var scope = serviceProvider.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<IDbTransaction>>();
        await unitOfWork.StartTransactionAsync();

        var externalRoleMappingRepository = scope.ServiceProvider.GetRequiredService<IExternalRoleMappingRepository>();
        var mappings = await externalRoleMappingRepository.GetExternalRoleMappingsByExternalRolesAsync(roles.ToArray());
        
        var identityId = principal.GetIdentityId();
        identity.IdentityId = identityId;
        
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, identityId));

        List<RoleClaim> roleClaims = [];
        foreach (var role in mappings.Where(map => roles.Any(r => r == map.ExternalRole)))
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
            roleClaims.Add(new RoleClaim(role.Role, role.NamespaceId.ToString()));
        }
        
        principal.AddIdentity(claimsIdentity);
        identity.RoleClaims = roleClaims;

        return principal;
    }
    
    private static List<string> GetRolesFromPrincipal(ClaimsPrincipal principal)
    {
        var resourceAccessClaim = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrEmpty(resourceAccessClaim))
        {
            return [];
        }

        using var doc = JsonDocument.Parse(resourceAccessClaim);
        var roles = doc.RootElement
            .GetProperty("roles")
            .EnumerateArray()
            .Select(role => role.GetString())
            .Where(role => role is not null)
            .ToList();

        return roles!;
    }
}