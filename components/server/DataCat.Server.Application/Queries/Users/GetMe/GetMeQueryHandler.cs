namespace DataCat.Server.Application.Queries.Users.GetMe;

public sealed class GetMeQueryHandler(IIdentity identity) : IQueryHandler<GetMeQuery, GetMeResponse>
{
    public Task<Result<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var result = new GetMeResponse(
            identity.IdentityId,
            identity.RoleClaims.Select(x => new UserClaim(x.Role.Name.ToLower(), x.NamespaceId)).ToList());
        
        return Task.FromResult(Result.Success(result));
    }
}