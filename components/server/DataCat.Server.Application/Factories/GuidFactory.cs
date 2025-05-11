namespace DataCat.Server.Application.Factories;

public sealed class GuidFactory : IIdFactory<Guid>
{
    public Guid NewId() => Guid.NewGuid();
}