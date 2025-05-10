namespace DataCat.Server.Application.Queries.Users.GetMe;

public sealed record GetMeResponse(
    string IdentityId,
    IReadOnlyCollection<UserClaim> Claims);

public sealed record UserClaim(string Role, string NamespaceId);
