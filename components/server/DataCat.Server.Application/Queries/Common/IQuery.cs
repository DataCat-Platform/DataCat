namespace DataCat.Server.Application.Queries.Common;

public interface IQuery;

public interface IQuery<TResponse> : IQuery, IRequest<Result<TResponse>>;