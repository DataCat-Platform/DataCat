namespace DataCat.Auth.Keycloak.Services;

public sealed class DataCatClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true } ||
            principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        using var scope = serviceProvider.CreateScope();

        // var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();
        //
        // var identityId = principal.GetIdentityId();
        //
        // var userRoles = await authorizationService.GetRolesForUserAsync(identityId);
        //
        // var claimsIdentity = new ClaimsIdentity();
        //
        // claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));
        //
        // foreach (var role in userRoles.Roles)
        // {
        //     claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        // }
        //
        // principal.AddIdentity(claimsIdentity);

        return principal;
    }
}