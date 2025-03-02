namespace DataCat.Server.Application.Queries.User.Get;

public sealed record GetUserQuery(Guid UserId) : IRequest<Result<UserEntity>>, IAuthorizedQuery;
