namespace DataCat.Server.Application.Commands.Common;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;