namespace DataCat.Server.Application.Queries.Users.Get;

public sealed record GetUserQuery(Guid UserId) : IQuery<User>, IAuthorizedQuery;
