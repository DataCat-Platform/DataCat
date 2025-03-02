namespace DataCat.Server.Application.Commands.Alert.MuteAlert;

public sealed record MuteAlertCommand : IRequest<Result>, IAuthorizedCommand
{
    public required string Id { get; init; }
}