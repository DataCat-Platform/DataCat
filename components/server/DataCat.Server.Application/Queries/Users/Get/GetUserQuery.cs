namespace DataCat.Server.Application.Queries.Users.Get;

public sealed record GetUserQuery(Guid UserId) : IRequest<Result<User>>, IAuthorizedQuery;
