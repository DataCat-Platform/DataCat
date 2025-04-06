namespace DataCat.Server.Application.Auth;

public sealed record RoleClaim(UserRole Role, Guid Namespace);
